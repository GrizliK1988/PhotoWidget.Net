using System.Collections.Generic;

namespace PhotoWidget.Service.Repository
{
    public interface IRepository<T, in TId>
    {
        IEnumerable<T> Get();

        T Get(TId id);

        T Save(T entity);

        void Delete(T entity);

        void Delete(TId id);
    }
}
