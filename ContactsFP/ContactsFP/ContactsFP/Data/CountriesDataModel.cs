using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ContactsFP.Data
{
    public class CountriesDataModel
    {
        private IList<Country> _countries = new ObservableCollection<Country>();

        public CountriesDataModel()
        {
            _countries = new ObservableCollection<Country>();
        }

        public IList<Country> Countries
        {
            get
            {
                return _countries;
            }
            set
            {
                Countries = _countries;
            }
        }
    }
}
