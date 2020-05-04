using System.Collections.Immutable;
using System.Linq;
using System.Diagnostics.Contracts;
using TAPI2.Entities;
using TAPI2.Models;
using System;
using System.Collections.Generic;

namespace TAPI2.Extensions
{
    public static class ContactDtoExtension
    {
        public static Contact ToNewContact(this ContactDto contactDto)
        {
            Contact contact = new Contact(contactDto.Name, contactDto.Name2);
            foreach(var addr in contactDto.Addresses)
                contact.AddAddress(addr.Name, addr.Line1, addr.Line2);
            return contact;            
        } 

        public static Contact UpdateContact(this ContactDto contactDto, Contact contactToUpdate)
        {                        
            contactToUpdate.UpdateName(contactDto.Name);
            contactToUpdate.UpdateName2(contactDto.Name2);
            
            // remove addresses 
            var addrToDelete = contactToUpdate.Addresses
                                .Where(a => contactDto.Addresses.Exists(e => e.ID == a.ID) == false)
                                .ToList();              
            foreach(var addr in addrToDelete)
                contactToUpdate.DeleteAddress(addr.ID);

            // Update addresses             
            foreach(var addr in contactDto.Addresses)
            {
                Address ad = contactToUpdate.Addresses.FirstOrDefault(a => a.ID == addr.ID);
                if (ad != null)
                    addr.UpdateAddress(ad);
                else 
                    contactToUpdate.AddAddress(addr.Name, addr.Line1, addr.Line2);   
            }

            return contactToUpdate;
        }           
    }
}