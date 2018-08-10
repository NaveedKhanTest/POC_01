//using POC.Core;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;

//namespace POC.Data
//{
//    public class Repository<T> : IRepository<T> where T : BaseEntity
//    {
//        private readonly IDbContext _context;
//        private DbSet<T> _entities;

//        public Repository(IDbContext context)
//        {
//            this._context = context;
//        }

//        public T GetById(object id)
//        {
//            return this.Entities.Find(id);
//        }

//        public void Insert(T entity)
//        {
//            if (entity == null)
//            {
//                throw new ArgumentNullException("entity");
//            }
//            this.Entities.Add(entity);
//            this._context.SaveChanges();

//            //try
//            //{
//            //    if (entity == null)
//            //    {
//            //        throw new ArgumentNullException("entity");
//            //    }
//            //    this.Entities.Add(entity);
//            //    this._context.SaveChanges();
//            //}
//            //catch (DbEntityValidationException dbEx)
//            //{
//            //    var msg = string.Empty;

//            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
//            //    {
//            //        foreach (var validationError in validationErrors.ValidationErrors)
//            //        {
//            //            msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
//            //        }
//            //    }

//            //    var fail = new Exception(msg, dbEx);
//            //    throw fail;
//            //}
//        }

//        public void Update(T entity)
//        {
//            if (entity == null)
//            {
//                throw new ArgumentNullException("entity");
//            }
//            this._context.SaveChanges();

//            //try
//            //{
//            //    if (entity == null)
//            //    {
//            //        throw new ArgumentNullException("entity");
//            //    }
//            //    this._context.SaveChanges();
//            //}
//            //catch (DbEntityValidationException dbEx)
//            //{
//            //    var msg = string.Empty;
//            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
//            //    {
//            //        foreach (var validationError in validationErrors.ValidationErrors)
//            //        {
//            //            msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
//            //        }
//            //    }
//            //    var fail = new Exception(msg, dbEx);
//            //    throw fail;
//            //}
//        }

//        public void Delete(T entity)
//        {
//            if (entity == null)
//            {
//                throw new ArgumentNullException("entity");
//            }
//            this.Entities.Remove(entity);
//            this._context.SaveChanges();

//            //try
//            //{
//            //    if (entity == null)
//            //    {
//            //        throw new ArgumentNullException("entity");
//            //    }
//            //    this.Entities.Remove(entity);
//            //    this._context.SaveChanges();
//            //}
//            //catch (DbEntityValidationException dbEx)
//            //{
//            //    var msg = string.Empty;

//            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
//            //    {
//            //        foreach (var validationError in validationErrors.ValidationErrors)
//            //        {
//            //            msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
//            //        }
//            //    }
//            //    var fail = new Exception(msg, dbEx);
//            //    throw fail;
//            //}
//        }

//        public virtual IQueryable<T> Table
//        {
//            get
//            {
//                return this.Entities;
//            }
//        }

//        private DbSet<T> Entities
//        //private IDbSet<T> Entities
//        {
//            get
//            {
//                if (_entities == null)
//                {
//                    _entities = _context.Set<T>();
//                }
//                return _entities;
//            }
//        }
//    }
//}
