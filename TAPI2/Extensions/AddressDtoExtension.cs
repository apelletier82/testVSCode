using TAPI2.Entities;
using TAPI2.Models;

namespace TAPI2.Extensions
{
    public static class AddressDtoExtension
    {
        public static Address ToNewAddress(this AddressDto addressDto, long contactId)
            => new Address(contactId, addressDto.Name, addressDto.Line1, addressDto.Line2);

        public static Address UpdateAddress(this AddressDto addressDto, Address addressToUpdate)
        {
            addressToUpdate.UpdateLine1(addressDto.Line1);
            addressToUpdate.UpdateLine2(addressDto.Line2);
            addressToUpdate.UpdateName(addressDto.Name);
            
            return addressToUpdate;
        }        
    }
}