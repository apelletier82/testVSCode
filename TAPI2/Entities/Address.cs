using System;

namespace TAPI2.Entities
{
    public class Address
    {
        public long ID { get; private set; }
        public long ContactID { get; private set; }        
        public string Name { get; private set; }
        public string Line1 { get; private set; }
        public String Line2 { get; private set; }

        public void UpdateName(string name) => Name = name;
        public void UpdateLine1(string line1) => Line1 = line1;
        public void UpdateLine2(String line2) => Line2 = line2;

        public Address(long contactID, string name, string line1, String line2)
        {            
            ContactID = contactID;
            Name = name;
            Line1 = line1;
            Line2 = line2;
        }

        public Address(long contactID, string name, string line1)
            : this(contactID, name, line1, null) { }
    }
}