using System.Threading;
using System.Threading.Tasks;

namespace TAPI2.Services.Abstract
{
    public interface IEntityAddService<T>        
        where T: class
    {
        T Add(T instance);
        Task<T> AddAsync(T instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}