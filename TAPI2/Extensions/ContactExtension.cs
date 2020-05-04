using System.Collections.Generic;
using TAPI2.Entities;
using TAPI2.Models;

namespace TAPI2.Extensions
{
    public static class ContactExtension
    {
        public static ContactLightDto ToContactLightDto(this Contact contact)
            => new ContactLightDto()
            {
                ID = contact.ID,
                RowVersion = contact.RowVersion,
                Name = contact.Name,
                Name2 = contact.Name2,
                LastChangeDate = contact.ChangeDate                 
            };                    

        public static ContactDto ToContactDto(this Contact contact)
        {
            ContactDto dto = new ContactDto()
            {
                ID = contact.ID,
                RowVersion = contact.RowVersion,
                Name = contact.Name,
                Name2 = contact.Name2,
                LastChangeDate = contact.ChangeDate
            };
            
            dto.Addresses = new List<AddressDto>();
            foreach(var addr in contact.Addresses)            
                dto.Addresses.Add(addr.ToAddressDto());            

            return dto;
        }
    }
}