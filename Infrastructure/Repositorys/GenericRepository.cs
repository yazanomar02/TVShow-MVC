using Domain.Models;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositorys
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        public readonly TVDbContext context;
        public GenericRepository(TVDbContext context)
        {
            this.context = context;
        }


        public async Task<T>? AddAsync(T entity)
        {
            var NewEntity = await context.AddAsync(entity);
            return NewEntity.Entity;
        }

        public async Task<T>? GetAsync(Guid id, bool isIncludeTVShow = false)
        {
            IQueryable<T> query = context.Set<T>();

            if (isIncludeTVShow && typeof(T) == typeof(Attachment))
            {
                query = query.Include("TVShow"); // إذا كان الكيان Attachment يتم تضمين TVShow
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "AttachmentId") == id);
        }

        public async Task<IList<T>> GetAllAsync(bool isInclude = false)
        {
            IQueryable<T> query = context.Set<T>();

            if (isInclude)
            {
                query = query.Include("RelatedEntity"); // تضمين الكيانات المرتبطة بالكيان الاساسي
            }

            return await query.ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public T Update(T entity)
        {
            return context.Update(entity).Entity;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id) != null;
        }
    }
}
