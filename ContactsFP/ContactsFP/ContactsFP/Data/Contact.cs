using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsFP.Data
{
    public class Contact
    {
        public string Company { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public byte[] Photo { get; set; }

        private List<PhoneNumber> _phoneNumbers;
        public List<PhoneNumber> PhoneNumbers
        {
            get
            {
                if (_phoneNumbers == null)
                    _phoneNumbers = new List<PhoneNumber>();
                return _phoneNumbers;
            }
            set
            {
                _phoneNumbers = value;
            }
        }

        private List<string> _emailsAddress;
        public List<string> EmailsAddress { 
            get
            {
                if (_emailsAddress == null)
                    _emailsAddress = new List<string>();
                return _emailsAddress;
            }
            set
            {
                _emailsAddress = value;
            }
        }

        public Contact(string name, string lastName, string company)
        {
            Company = company;
            Name = name;
            LastName = lastName;
        }

        public override string ToString()
        {
            return Name;
        }

    }

}
