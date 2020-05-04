using TAPI2.Entities;
using TAPI2.Models;

namespace TAPI2.Extensions
{
    public static class AddressExtension
    {
        public static AddressDto ToAddressDto(this Address address) =>  
            new AddressDto() 
            {
                ID = address.ID,
                Name = address.Name,
                Line1 = address.Line1,
                Line2 = address.Line2
            };        
    }
}