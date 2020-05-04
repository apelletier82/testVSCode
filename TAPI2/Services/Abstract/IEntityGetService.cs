using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TAPI2.Services.Abstract
{
    public interface IEntityGetService<T>
        where T: class
    {
        T GetByID(long id);
        Task<T> GetByIDAsync(long id, CancellationToken cancellationToken = default(CancellationToken));
        T FindVersion(long id, byte[] rowVersion);
        Task<T> FindVersionAsync(long id, byte[] rowVersion, 
            CancellationToken cancellation = default(CancellationToken));     
        IEnumerable<T> List();
        Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}