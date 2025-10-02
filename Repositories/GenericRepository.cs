using Library_Management_Sys.Models;
using Library_Management_Sys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Library_Management_Sys.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
            private readonly LibraryDbContext _db;
            internal DbSet<T> dbSet;
            public GenericRepository(LibraryDbContext db)
            {
                _db = db;
                this.dbSet = _db.Set<T>();
            }

            public async Task CreateAsync(T entity)
            {
                await dbSet.AddAsync(entity);
            }

            public async Task CreateAndSaveAsync(T entity)
            {
                await dbSet.AddAsync(entity);
                await SaveAsync();
            }

            public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includeProperties)
            {
                IQueryable<T> query = dbSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query = query.Include(includeProperty);
                    }
                }

                return await query.ToListAsync();
            }

            public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties)
            {
                IQueryable<T> query = dbSet;
               
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (includeProperties != null)
                {
                    query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                }

                return await query.FirstOrDefaultAsync();
            }
            public async Task UpdateAsync(T entity)
            {
                dbSet.Update(entity);
                _db.Entry(entity).State = EntityState.Modified;
                await SaveAsync();
            }

            public async Task RemoveAsync(T entity)
            {
                dbSet.Remove(entity);
                await SaveAsync();
            }

            public async Task SaveAsync()
            {
                await _db.SaveChangesAsync();
            }
    }
}
