﻿using Microsoft.EntityFrameworkCore;
using RoverCore.Abstractions.Templates;
using Scheduler.Domain.Entities.Templates;
using Scheduler.Infrastructure.Common.Templates.Models;
using Scheduler.Infrastructure.Persistence.DbContexts;
using Serviced;

namespace Scheduler.Infrastructure.Common.Templates.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly VirtualFileProvider _virtualFileProvider;
        private readonly ApplicationDbContext _context;

        public TemplateService(VirtualFileProvider virtualFileProvider, ApplicationDbContext context)
        {
            _virtualFileProvider = virtualFileProvider;
            _context = context;

            if (_virtualFileProvider.Templates.Count == 0)
            {
                LoadTemplates();
            }
        }

        public IQueryable<ITemplate> GetTemplateQueryable()
        {
            return _context.Template;
        }

        public async Task<ITemplate?> FindTemplateById(int id)
        {
            return await _context.Template.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ITemplate?> FindTemplateBySlug(string slug)
        {
            return await _context.Template.FirstOrDefaultAsync(m => m.Slug == slug);
        }

        public async Task<ITemplate?> CreateTemplate(ITemplate template)
        {
            template.Created = DateTime.UtcNow;
            template.Updated = DateTime.UtcNow;

            _context.Add(template);
            await _context.SaveChangesAsync();

            _virtualFileProvider.Templates.Add((Template)template);

            return template;
        }

        public async Task<ITemplate?> UpdateTemplate(ITemplate template)
        {
            var model = await FindTemplateById(template.Id);

            if (model == null)
            {
                return null;
            }

            model.Slug = template.Slug;
            model.Name = template.Name;
            model.Description = template.Description;
            model.Body = template.Body;
            model.PreHeader = template.PreHeader;
            model.Updated = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var tpl = _virtualFileProvider.Templates.Find(x => x.Id == template.Id);
            if (tpl != null)
            {
                tpl.Slug = model.Slug;
                tpl.Name = model.Name;
                tpl.Description = model.Description;
                tpl.Body = model.Body;
                tpl.PreHeader = model.PreHeader;
                tpl.Updated = model.Updated;
            }
            else
            {
                _virtualFileProvider.Templates.Add((Template)template);
            }

            return model;
        }

        public async Task DeleteTemplate(int id)
        {
            var template = await FindTemplateById(id);
            if (template != null)
            {
                _context.Template.Remove((Template)template);
                await _context.SaveChangesAsync();
            }
        }

        public bool TemplateExists(int id)
        {
            return _context.Template.Any(e => e.Id == id);
        }

        public bool TemplateExists(string slug)
        {
            return _context.Template.Any(e => e.Slug == slug);
        }

        public void LoadTemplates()
        {
            _virtualFileProvider.Templates.Clear();
            _virtualFileProvider.Templates.AddRange(_context.Template.ToList());
        }
    }
}
