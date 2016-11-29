using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Mvvm;
using ContactsFP.Data;
using ContactsFP.Views;
using ContactsFP.Services;

namespace ContactsFP.DataView
{

    [ViewType(typeof(SendContact))]
    public class SendViewModel : ViewModel
    {
        public SendViewModel()
        {

        }

        private RelayCommand _sendCommand;
        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new RelayCommand(
                    async () => await SendContact(),

                    () => true));
            }
        }

        private async Task SendContact()
        {
            var contacts = ViewDataModel.Contacts;
            if (await WebService.CreateContacts(contacts))
            {
                await App.Current.MainPage.DisplayAlert("Confirmación", "Se envio la información exitosamente", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Confirmación", "No se pudo enviar la información exitosamente", "OK");
            }
        }

    }
}
