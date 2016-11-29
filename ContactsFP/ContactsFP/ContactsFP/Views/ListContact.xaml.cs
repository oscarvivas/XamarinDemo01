using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsFP.Data;
using ContactsFP.DataView;
using Xamarin.Forms;

namespace ContactsFP.Views
{
    public partial class ListContact : ContentPage
    {
        public ListContact()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = ViewDataModel.Contacts.Contacts;
            
        }

    }
}
