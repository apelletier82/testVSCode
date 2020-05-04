using System;

namespace TAPI2.Models
{
    public class ContactLightDto
    {
        public long ID { get; set; }
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public String Name2 { get; set; }
        public DateTimeOffset? LastChangeDate {get; set;}        
    }
}