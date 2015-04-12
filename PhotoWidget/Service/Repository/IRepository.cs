using System.Collections.Generic;

namespace PhotoWidget.Service.Repository
{
    public interface IRepository<T, in TId>
    {
        IEnumerable<T> Get();

        T Get(TId id);

        void Save(ref T entity);

        void Delete(T entity);

        void Delete(TId id);
    }
}
