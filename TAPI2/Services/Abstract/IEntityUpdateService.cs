using System.Threading;
using System.Threading.Tasks;

namespace TAPI2.Services.Abstract
{
    public interface IEntityUpdateService<T>
        where T:class
    {
         T Update(T instance);
         Task<T> UpdateAsync(T instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}