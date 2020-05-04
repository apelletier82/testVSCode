using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TAPI2.Entities;

namespace TAPI2.Services.Abstract
{
    public interface IContactService :
        IEntityGetService<Contact>,
        IEntityAddService<Contact>,
        IEntityUpdateService<Contact>,
        IEntityDeleteService<Contact>
    { 
        IEnumerable<Contact> ListByName(string name, bool strictEquality);
        Task<IEnumerable<Contact>> ListByNameAsync(string name, bool strictEquality,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}