using System;
using System.Collections.Generic;

namespace TAPI2.Models
{
    public class ContactDto: ContactLightDto
    {
        public List<AddressDto> Addresses { get; set; }
    }
}