﻿using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using RoverCore.Abstractions.Templates;
using Scheduler.Domain.Entities.Templates;

namespace Scheduler.Infrastructure.Common.Templates.Models
{
    /// <summary>
    /// This class allows FluentEmail to include templates that are stored in the database by using their slug names as a file path.
    /// </summary>
    public class VirtualFileProvider : IFileProvider
    {
        public List<Template> Templates { get; set; }

        public VirtualFileProvider()
        {
            Templates = new List<Template>();
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            const string liquidExtension = ".liquid";
            if (subpath.EndsWith(liquidExtension))
            {
                subpath = Path.GetFileNameWithoutExtension(subpath);
            }

            var template = Templates.Find(t => t.Slug == subpath);

            return new VirtualFileInfo(template);
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }

}
