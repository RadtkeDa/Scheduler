﻿using Microsoft.EntityFrameworkCore;
using Scheduler.Infrastructure.Persistence.DbContexts;

namespace Scheduler.Infrastructure.Persistence.Extensions
{
    public static class ContextExtensions
    {
        /// <summary>
        ///     Determines whether to add an entity for ef tracking or update it
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="entity"></param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void AddOrUpdate(this ApplicationDbContext ctx, object entity)
        {
            var entry = ctx.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    ctx.Add(entity);
                    break;
                case EntityState.Modified:
                    ctx.Update(entity);
                    break;
                case EntityState.Added:
                    ctx.Add(entity);
                    break;
                case EntityState.Unchanged:
                    // item already in db no need to do anything  
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}