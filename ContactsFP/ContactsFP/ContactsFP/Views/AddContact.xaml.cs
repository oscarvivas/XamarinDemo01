using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsFP.DataView;

using Xamarin.Forms;

namespace ContactsFP.Views
{
    public partial class AddContact : ContentPage
    {
        public AddContact()
        {
            InitializeComponent();

            BindingContext = ViewModelLocator.AddModel;
        }
    }
}
