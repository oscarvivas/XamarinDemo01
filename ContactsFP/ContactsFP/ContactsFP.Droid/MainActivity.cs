
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Serialization;
using Xamarin.Forms.Labs.Caching.SQLiteNet;
using System.IO;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Droid;

namespace ContactsFP.Droid
{
    [Activity(Label = "ContactsFP", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static bool _initialized;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            if (!_initialized)
            {
                this.SetIoc();
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }


        /// <summary>
        /// Sets the IoC.
        /// </summary>
        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            var app = new XFormsAppDroid();

            var documents = Environment.ExternalStorageDirectory.AbsolutePath;
            var pathToDatabase = Path.Combine(documents, "xforms.db");

            //app.Init(this);

            resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<IJsonSerializer, Xamarin.Forms.Labs.Services.Serialization.JsonNET.JsonSerializer>()
                .Register<IDependencyContainer>(resolverContainer)
                .Register<IXFormsApp>(app);
                //.Register<ISimpleCache>(t => new SQLiteSimpleCache(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(),
                //    new SQLite.Net.SQLiteConnectionString(pathToDatabase, true), t.Resolve<IJsonSerializer>()));


            Resolver.SetResolver(resolverContainer.GetResolver());

            _initialized = true;
        }
    }
}

