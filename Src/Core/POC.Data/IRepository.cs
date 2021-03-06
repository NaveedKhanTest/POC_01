﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace POC.Data
{
    public interface IRepository<T>
{
    T Get<TKey>(TKey id);
    IQueryable<T> GetAll();

      Task<T> PostAsync(T entity);
      T Update(T entity);


        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);


        void Add(T entity);

        #region Async 

        Task<IQueryable<T>> GetAllAsync();

        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindByConditionAync(Expression<Func<T, bool>> expression);

        Task SaveAsync();
        object FindByConditionAync(int id);

        #endregion


        //IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,string includeProperties = "");

    }

}







//using POC.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace POC.Data
//{
//    public interface IRepository<T> where T : BaseEntity
//    {
//        T GetById(object id);
//        void Insert(T entity);
//        void Update(T entity);
//        void Delete(T entity);
//        IQueryable<T> Table { get; }
//    }
//}
