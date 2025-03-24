using Cs.Unicam.TrashHunter.Models.Repositorys;
using Cs.Unicam.TrashHunter.Models.Repositorys.Models;

namespace Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        Task<T> Find(object id);
        Task<IEnumerable<T>> GetAll();
        Task Save();
        void Update(T entity);
        Task<bool> Exist(object id);
        Task<PagingResult<T>> GetFiltered(IFilter filter, int skip, int take);
    }
}