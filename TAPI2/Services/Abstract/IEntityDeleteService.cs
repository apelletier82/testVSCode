using System.Threading;
using System.Threading.Tasks;

namespace TAPI2.Services.Abstract
{
    public interface IEntityDeleteService<T>
        where T: class
    {
        bool Delete(T instance);
        bool Delete(long id);
        Task<bool> DeleteAsync(T instance, 
            CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteAsync(long id, 
            CancellationToken cancellationToken = default(CancellationToken));
    }
}