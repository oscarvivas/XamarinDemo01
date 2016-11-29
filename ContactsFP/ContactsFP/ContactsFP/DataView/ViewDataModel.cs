using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsFP.Data;

namespace ContactsFP.DataView
{
    public class ViewDataModel
    {
        private static ContactsDataModel _contacts;
        public static ContactsDataModel Contacts
        {
            get
            {
                if (_contacts == null)
                    _contacts = new ContactsDataModel();
                return _contacts;
            }
        }

        private static CountriesDataModel _countries;
        public static CountriesDataModel Countries
        {
            get
            {
                if (_countries == null)
                    _countries = new CountriesDataModel();
                return _countries;
            }
        }

    }
}
