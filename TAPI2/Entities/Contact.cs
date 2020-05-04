using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TAPI2.Entities
{
    public class Contact
    {
        private List<Address> _addresses;
        public long ID { get; private set; }
        public byte[] RowVersion { get; private set; }
        public string Name { get; private set; }
        public String Name2 { get; private set; }

        public string CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public String ChangeUser { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }

        public IReadOnlyCollection<Address> Addresses { get => new ReadOnlyCollection<Address>(_addresses); }

        public Contact(string name, String name2)
        {
            Name = name;
            Name2 = name2;
            _addresses = new List<Address>();
        }

        public Contact(string name) : this(name, null) { }

        public void UpdateName(string name) => Name = name;

        public void UpdateName2(String name2) => Name2 = name2;

        public bool AddressExists(string name) => (FindAddress(name) != null);

        public Address FindAddress(string name) =>
            _addresses.SingleOrDefault(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public Address GetAddressByName(string name)
        {
            var addr = FindAddress(name);
            if (addr == null)
                throw new NullReferenceException(
                            string.Format("Address with name {0} does not exists", name));
            return addr;
        }

        public void AddAddress(string name, string line1, String line2)
        {
            if (!AddressExists(name))
                _addresses.Add(new Address(ID, name, line1, line2));
        }

        public void UpdateAddress(string name, string line1, String line2)
        {
            var addr = GetAddressByName(name);
            addr.UpdateLine1(line1);
            addr.UpdateLine2(line2);
        }

        public void RenameAddress(string currentName, string newName)
        {
            GetAddressByName(currentName).UpdateName(newName);
        }

        public void DeleteAddress(string name)
        {
            var addr = GetAddressByName(name);
            if (addr != null)
                _addresses.Remove(addr);
        }

        public void DeleteAddress(long id)
        {
            var addr = _addresses.FirstOrDefault(a => a.ID == id);
            if (addr != null)
                _addresses.Remove(addr);
        }
    }
}