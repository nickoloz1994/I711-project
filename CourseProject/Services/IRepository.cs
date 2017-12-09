using System.Collections.Generic;
using System.Linq;

namespace CourseProject.Services
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);

        IEnumerable<TEntity> GetAll();

        //IQueryable<TEntity> GetEntities();

        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        void Save();
    }
}