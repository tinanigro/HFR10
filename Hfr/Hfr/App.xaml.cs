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
using Hfr.ViewModel;
using Hfr.Helpers;
using Hfr.Services;
using Hfr.Model;

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

        public static Frame NavigationFrame => AppShell?.NavigationFrame;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Loc.Init();
#if DEBUG
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            if (AppShell == null)
            {
                AppShell = new Shell();
                Loc.NavigationService.Initialize();
                ThreadUI.setDispatcher(NavigationFrame.Dispatcher);
                AppViewHelper.SetAppView();
                Window.Current.Activate();

                Loc.Main.AccountManager = new AccountManager();

                AppShell.Language = ApplicationLanguages.Languages[0];

                AppShell.NavigationFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
            }
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
