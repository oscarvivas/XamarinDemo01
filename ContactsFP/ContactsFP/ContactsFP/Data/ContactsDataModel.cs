using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContactsFP.Data
{
    public class ContactsDataModel
    {
        public Location Location = new Location();

        private IList<Contact> _contacts = new ObservableCollection<Contact>();

        public ContactsDataModel()
        {
            _contacts = new ObservableCollection<Contact>();
        }

        public IList<Contact> Contacts
        {
            get
            {
                return _contacts;
            }
            set
            {
                Contacts = _contacts;
            }
        }

    }
}
