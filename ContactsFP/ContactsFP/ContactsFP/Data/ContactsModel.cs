using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContactsFP.Data
{
    public class ContactsModel
    {
        public List<Contact> Contacts = new List<Contact>();

        public Location Location = new Location();

        public string RegisteredBy = Constants.registeredBy;

        public int Type = Constants.type;
    }
}
