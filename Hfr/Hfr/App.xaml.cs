using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;
using Hfr.Views;
using Hfr.Views.MainPages;
using Hfr.ViewModel;
using Hfr.Helpers;
using Hfr.Services;

namespace Hfr
{
    sealed partial class App : Application
    {
        public TelemetryClient TelemetryClient = new TelemetryClient();
        public static Shell AppShell
        {
            get { return (Window.Current.Content as Shell); }
            private set { Window.Current.Content = value; }
        }

        public static Frame NavigationFrame
        {
            get { return AppShell?.NavigationFrame; }
        }
        
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            if (AppShell == null)
            {
                AppShell = new Shell();
                Loc.NavigationService.Initialize(NavigationFrame);
                ThreadUI.setDispatcher(NavigationFrame.Dispatcher);
                Loc.Main.AccountManager = new AccountManager();

                AppShell.Language = ApplicationLanguages.Languages[0];

                AppShell.NavigationFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
            }
            AppShell.NavigationFrame.Navigate(typeof(MainPage), e.Arguments);
            Window.Current.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
