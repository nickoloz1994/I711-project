using System.Collections.Generic;
using System.Linq;
using CourseProject.Data;

namespace CourseProject.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext _context;
        //protected DbSet<TEntity> DBSet { get; set; }

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            //DBSet = _context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public virtual void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        //public virtual IQueryable<TEntity> GetEntities()
        //{
        //    var res = _context.Set<TEntity>().AsQueryable<TEntity>();
        //    return res;
        //}

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}