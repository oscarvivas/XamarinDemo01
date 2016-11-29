using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Labs.iOS.Controls.Calendar;
using Xamarin.Forms.Labs.Services.Serialization;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.iOS;
using Xamarin.Forms.Labs.Services;
using System.IO;
using Xamarin.Forms.Labs.Mvvm;

namespace ContactsFP.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            this.SetIoc();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// Sets the IoC.
        /// </summary>
        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            var app = new XFormsAppiOS();
            //app.Init(this);

            var documents = app.AppDataDirectory;
            var pathToDatabase = Path.Combine(documents, "xforms.db");

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<IJsonSerializer, Xamarin.Forms.Labs.Services.Serialization.JsonNET.JsonSerializer>()
                .Register<IXFormsApp>(app)
                .Register<IDependencyContainer>(t => resolverContainer);
                //.Register<ISimpleCache>(
                //    t => new SQLiteSimpleCache(new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS(),
                //        new SQLite.Net.SQLiteConnectionString(pathToDatabase, true), t.Resolve<IJsonSerializer>()));

            Resolver.SetResolver(resolverContainer.GetResolver());
        }

    }
}
