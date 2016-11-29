using System;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Services.Geolocation;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContactsFP.Views;
using ContactsFP.Data;
using ContactsFP.Services;

namespace ContactsFP.DataView
{
    [ViewType(typeof(AddContact))]
    public class AddViewModel : ViewModel
    {
        private IGeolocator geolocator;
        private TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private CancellationTokenSource cancelSource;

        public AddViewModel()
        {
            Setup();
        }

        #region fields

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                this.ChangeAndNotify(ref _name, value);
            }
        }
        private string _lastName = string.Empty;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                this.ChangeAndNotify(ref _lastName, value);
            }
        }
        private string _company = string.Empty;
        public string Company
        {
            get
            {
                return _company;
            }
            set
            {
                this.ChangeAndNotify(ref _company, value);
            }
        }
        private string _email = string.Empty;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                this.ChangeAndNotify(ref _email, value);
            }
        }
        private string _phone = string.Empty;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                this.ChangeAndNotify(ref _phone, value);
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand(
                    async () => await AddContact(),

                    () => true));
            }
        }
        private async Task AddContact()
        {
            if (Name.Equals(String.Empty) || LastName.Equals(String.Empty) || Company.Equals(String.Empty) || Email.Equals(String.Empty) || Phone.Equals(String.Empty))
            {
                await App.Current.MainPage.DisplayAlert("Validación", "Todos los campos son obligatorios", "OK");
                return;
            }
            if (!ViewUtil.IsValidEmailId(Email))
            {
                await App.Current.MainPage.DisplayAlert("Validación", "El formato del correo no es valido", "OK");
                return;
            }
            if (_imageSource == null)
            {
                await App.Current.MainPage.DisplayAlert("Validación", "Tome una foto o carguela de la galeria de imagenes", "OK");
                return;
            }
            if (PositionStatus.Equals(String.Empty) || PositionLatitude.Equals(String.Empty) || PositionLongitude.Equals(String.Empty))
            {
                await App.Current.MainPage.DisplayAlert("Validación", "Por favor consulte su ubicación, recuerde habilitar su GPS", "OK");
                return;
            }

            Country country = new Country();
            country.Code = 57;
            country.Name = "Colombia";
            PhoneNumber phone = new PhoneNumber();
            phone.Country = country;
            phone.Number = Phone;
            Contact contact = new Contact(Name, LastName, Company);
            contact.EmailsAddress.Add(Email);
            contact.PhoneNumbers.Add(phone);
            contact.Photo = ViewUtil.ImageToByte(ImageSource);
            Location location = new Location();
            location.Latitude = Double.Parse(PositionLatitude);
            location.Longitude = Double.Parse(PositionLongitude);

            ViewDataModel.Contacts.Location = location;
            ViewDataModel.Contacts.Contacts.Add(contact);

            cleanFields();
            
            await App.Current.MainPage.DisplayAlert("Confirmación","Se agrego el contacto", "OK");
        }

        public void cleanFields()
        {
            Name = String.Empty;
            LastName = String.Empty;
            Company = String.Empty;
            Email = String.Empty;
            Phone = String.Empty;
            //PositionStatus = String.Empty;
            //PositionLatitude = String.Empty;
            //PositionLongitude = String.Empty;
        }


        #endregion

        #region image

        /// <summary>
        /// The _picture chooser
        /// </summary>
        private IMediaPicker _mediaPicker;
        /// <summary>
        /// The _image source
        /// </summary>
        private ImageSource _imageSource;
         /// <summary>
        /// The _take picture command
        /// </summary>
        private RelayCommand _takePictureCommand;
        /// <summary>
        /// The _select picture command
        /// </summary>
        private RelayCommand _selectPictureCommand;
 
        /// <summary>
        /// The _scheduler
        /// </summary>
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        //private CancellationTokenSource cancelSource;

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                this.ChangeAndNotify(ref _imageSource, value);
            }
        }

        /// <summary>
        /// Gets the take picture command.
        /// </summary>
        /// <value>The take picture command.</value>
        public RelayCommand TakePictureCommand
        {
            get
            {
                return _takePictureCommand ?? (_takePictureCommand = new RelayCommand(
                    async () => await TakePicture(),

                    () => true));
            }
        }

        /// <summary>
        /// Gets the select picture command.
        /// </summary>
        /// <value>The select picture command.</value>
        public RelayCommand SelectPictureCommand
        {
            get
            {
                return _selectPictureCommand ?? (_selectPictureCommand = new RelayCommand(
                    async () => await SelectPicture(),

                    () => true));
            }
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        void Setup()
        {
            if (ViewDataModel.Countries.Countries.Count == 0)
            {
                //ViewDataModel.Countries.Countries = WebService.GetCountries().Result;
            }

            if (_mediaPicker != null)
            {
                return;
            }

            var device = Resolver.Resolve<IDevice>();

            _mediaPicker = DependencyService.Get<IMediaPicker>();
            //RM: hack for working on windows phone? 
            if (_mediaPicker == null)
                _mediaPicker = device.MediaPicker;
        }
        /// <summary>
        /// Takes the picture.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task TakePicture()
        {
            Setup();

            ImageSource = null;

            await this._mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    var s = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    var canceled = true;
                }
                else
                {
                    var mediaFile = t.Result;

                    ImageSource = ImageSource.FromStream(() => mediaFile.Source);

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }

        /// <summary>
        /// Selects the picture.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task SelectPicture()
        {
            Setup();

            ImageSource = null;
            try
            {
                var mediaFile = await this._mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                {
                    DefaultCamera = CameraDevice.Front,
                    MaxPixelDimension = 400
                });
                ImageSource = ImageSource.FromStream(() => mediaFile.Source);
            }
            catch (System.Exception ex)
            {

            }
        }
    
        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

#endregion


#region Localization

        private string _positionStatus = string.Empty;
        public string PositionStatus
        {
            get
            {
                return _positionStatus;
            }
            set
            {
                this.ChangeAndNotify(ref _positionStatus, value);
            }
        }
        private string _positionLatitude = string.Empty;
        public string PositionLatitude
        {
            get
            {
                return _positionLatitude;
            }
            set
            {
                this.ChangeAndNotify(ref _positionLatitude, value);
            }
        }
        private string _positionLongitude = string.Empty;
        public string PositionLongitude
        {
            get
            {
                return _positionLongitude;
            }
            set
            {
                this.ChangeAndNotify(ref _positionLongitude, value);
            }
        }

        private RelayCommand _getPositionCommand;
        public RelayCommand GetPositionCommand
        {
            get
            {
                return _getPositionCommand ?? new RelayCommand(
                    async () => { await GetPosition(); },
                    () => true);
            }
        }

        void SetupGPS()
        {
            if (this.geolocator != null)
                return;
            this.geolocator = DependencyService.Get<IGeolocator>();
            this.geolocator.PositionError += OnListeningError;
            this.geolocator.PositionChanged += OnPositionChanged;
        }
        async Task GetPosition()
        {
            try
            {
                SetupGPS();

                this.cancelSource = new CancellationTokenSource();

                PositionStatus = String.Empty;
                PositionLatitude = String.Empty;
                PositionLongitude = String.Empty;
                IsBusy = true;
                await this.geolocator.GetPositionAsync(timeout: 10000, cancelToken: this.cancelSource.Token, includeHeading: true)
                    .ContinueWith(t =>
                    {
                        IsBusy = false;
                        if (t.IsFaulted)
                            PositionStatus = ((GeolocationException)t.Exception.InnerException).Error.ToString();
                        else if (t.IsCanceled)
                            PositionStatus = "Canceled";
                        else
                        {
                            PositionStatus = "Latitud: " + t.Result.Latitude.ToString("N4") + "Longitud: " + t.Result.Longitude.ToString("N4");
                            PositionLatitude = t.Result.Latitude.ToString("N4");
                            PositionLongitude = t.Result.Longitude.ToString("N4");
                        }

                    }, scheduler);
            }
            catch
            {
                PositionStatus = "No se pudo obtener la ubicación";
                PositionLatitude = "0";
                PositionLongitude = "0";
            }
        }

        void CancelPosition()
        {
            CancellationTokenSource cancel = this.cancelSource;
            if (cancel != null)
                cancel.Cancel();
        }

        //		partial void ToggleListening (NSObject sender)
        //		{
        //			SetupGPS();
        //
        //			if (!this.geolocator.IsListening)
        //			{
        //				ToggleListeningButton.SetTitle ("Stop listening", UIControlState.Normal);
        //
        //				this.geolocator.StartListening (minTime: 30000, minDistance: 0, includeHeading: true);
        //			}
        //			else
        //			{
        //				ToggleListeningButton.SetTitle ("Start listening", UIControlState.Normal);
        //				this.geolocator.StopListening();
        //			}
        //		}

        private void OnListeningError(object sender, PositionErrorEventArgs e)
        {
            //			BeginInvokeOnMainThread (() => {
            //				ListenStatus.Text = e.Error.ToString();
            //			});
        }

        private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            //			BeginInvokeOnMainThread (() => {
            //				ListenStatus.Text = e.Position.Timestamp.ToString("G");
            //				ListenLatitude.Text = "La: " + e.Position.Latitude.ToString("N4");
            //				ListenLongitude.Text = "Lo: " + e.Position.Longitude.ToString("N4");
            //			});
        }        
#endregion

    }
}
