using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Jupiter.Application;
using Jupiter.Services.Navigation;
using Meridian.View;
using GalaSoft.MvvmLight.Ioc;
using VkLib;
using Meridian.Services;
using GalaSoft.MvvmLight.Threading;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;
using GalaSoft.MvvmLight.Messaging;
using Meridian.Utils.Messaging;
using Windows.ApplicationModel;
using System;
using Meridian.View.Common;
using Meridian.Enum;
using System.Globalization;
using Meridian.Utils.Helpers;
using Windows.UI.Popups;
using Windows.System;
using LastFmLib;

namespace Meridian
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            Ioc.Setup();

            if (AppState.Theme != null)
                RequestedTheme = AppState.Theme.Value;
        }

        public override async void OnStart(StartKind startKind, IActivatedEventArgs args)
        {
            Logger.AppStart();

            TimeBombHelper.Initialize();

            DispatcherHelper.Initialize();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var appView = ApplicationView.GetForCurrentView();
                appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }

            await AudioService.Instance.LoadState();

            var vk = SimpleIoc.Default.GetInstance<Vk>();
            vk.Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var lastFm = Ioc.Resolve<LastFm>();
            lastFm.SessionKey = AppState.LastFmSession?.Key;

            if (AppState.VkToken == null || AppState.VkToken.HasExpired)
            {
                NavigationService.Navigate(typeof(LoginView));
            }
            else
            {

                vk.AccessToken = AppState.VkToken;
                Messenger.Default.Send(new MessageUserAuthChanged { IsLoggedIn = true });

                if (AppState.StartPage == StartPage.Mymusic)
                    NavigationService.Navigate(typeof(MyMusicView));
                else
                    NavigationService.Navigate(typeof(ExploreView));
            }

            //if (TimeBombHelper.HasExpired())
            //{
            //    var dialog = new MessageDialog(Utils.Helpers.Resources.GetStringByKey("TimeBomb_ExpiredContent"), Utils.Helpers.Resources.GetStringByKey("TimeBomb_ExpiredTitle"));
            //    dialog.Commands.Add(new UICommand(Utils.Helpers.Resources.GetStringByKey("TimeBomb_Install"), async command =>
            //    {
            //        await Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9wzdncrdmsq3"));
            //    }));

            //    dialog.Commands.Add(new UICommand(Utils.Helpers.Resources.GetStringByKey("Close")));

            //    await dialog.ShowAsync();

            //    Application.Current.Exit();
            //}
        }

        public override async void OnSuspending(SuspendingEventArgs e)
        {
            base.OnSuspending(e);

            var deferral = e.SuspendingOperation.GetDeferral();

            TileHelper.UpdateIsPlaying(false);
            await AudioService.Instance.SaveState();

            deferral.Complete();
        }

        public override void OnInitialize(IActivatedEventArgs args)
        {
            //content may already be shell when resuming
            if (Window.Current.Content is Shell)
            {
                base.OnInitialize(args);
                return;
            }

            if (AppState.Accent != "Blue")
                Resources.MergedDictionaries[0].Source = new Uri($"ms-appx:///Resources/Accents/{AppState.Accent ?? "Blue"}.xaml");

            // setup hamburger shell
            var shell = new Shell();
            var navigationService = new NavigationService(shell.ContentFrame);
            navigationService.IsBackButtonEnabled = false;
            WindowWrapper.Current().NavigationService = navigationService;
            Window.Current.Content = shell;
        }
    }
}