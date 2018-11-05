using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace POC.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected DbSet<T> DbSet;

        public Repository(PocDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        //public async Task<Students> Create(Students students)
        //{
        //    var result = await this.DbContext.Students.AddAsync(students);
        //    DbContext.SaveChanges();

        //    return result.Entity;
        //}

        //SaveAsync
        public async Task<T> PostAsync(T entity)
        {
            var result = await Context.Set<T>().AddAsync(entity);
            Context.SaveChanges();
            return result.Entity;


            //var result = Context.Set<T>().AddAsync(entity);
            //Context.SaveChanges();
            ////return result.Entity;
            //Context.Set<T>().Add(entity);
            //Save();
        }

        public T Update(T entity)
        {
            try
            {

            var result = Context.Set<T>().Update(entity);
            Context.SaveChanges();
            return result.Entity;

            }
            catch (Exception ex)
            {
                var msg = ex.Message;

                throw;
            }
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);

            Save();
        }

        public T Get<TKey>(TKey id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        //async Task<IQueryable<URL>> GetAllUrlsAsync()
        public async Task<IQueryable<T>> GetAllAsync()
        {
            return DbSet;
        }


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.Context.Set<T>().Where(expression);//.ToListAsync();
        }


        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await this.Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByConditionAync(Expression<Func<T, bool>> expression)
        {
            return await this.Context.Set<T>().Where(expression).AsNoTracking().ToListAsync();
        }


        //public void Update(T entity)
        //{
        //    Save();
        //}



        private void Save()
        {
            Context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await this.Context.SaveChangesAsync();
        }

        public object FindByConditionAync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
