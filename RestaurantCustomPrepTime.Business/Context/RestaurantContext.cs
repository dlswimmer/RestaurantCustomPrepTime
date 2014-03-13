using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestaurantCustomPrepTime.Business.Entity;

namespace RestaurantCustomPrepTime.Business.Context
{
    internal class RestaurantContext : DbContext
    {
        public RestaurantContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RestaurantContext>());
        }

        public DbSet<CustomPrepTime> CustomPrepTimes { get; set; }
    }

    public interface IRestaurantContextAccess : IDisposable
    {
        IQueryable<T> Table<T>() where T : BaseEntity;
        void Insert<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
        void Delete<T>(IEnumerable<T> entities) where T : BaseEntity;
        void Save();
    }

    internal class RestaurantContextAccess : IRestaurantContextAccess
    {
        private readonly RestaurantContext _context;

        public RestaurantContextAccess()
        {
            _context = new RestaurantContext();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> Table<T>() where T : BaseEntity
        {
            return _context.Set<T>();
        }

        public void Insert<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Remove(entity);
        }

        public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
