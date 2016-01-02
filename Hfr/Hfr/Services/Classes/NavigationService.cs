using Windows.UI.Xaml.Controls;
using Hfr.Views.MainPages;
using Windows.UI.Core;
using Hfr.ViewModel;
using Hfr.Model;

namespace Hfr.Services.Classes
{
    public class NavigationService
    {
        private Frame _navigationFrame;
        public View CurrentView;
        public bool CanGoBack
        {
            get { return CanGoBackCompute(); }
        }

        public void Initialize(Frame rootFrame)
        {
            _navigationFrame = rootFrame;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                if (CanGoBack)
                {
                    e.Handled = true;
                    GoBack();
                }
            };
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (CanGoBack)
            {
                e.Handled = true;
                GoBack();
            }
        }

        public void ShowBackButtonIfCanGoBack()
        {
            if (CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }


        public void GoBack()
        {
            switch (CurrentView)
            {
                case View.Main:
                    if (Loc.Main.TopicVisible)
                    {
                        Loc.Main.SelectedTopic = -1;
                    }
                    break;
                case View.Editor:
                case View.Settings:
                    _navigationFrame.GoBack();
                    CurrentView = View.Main;
                    break;
            }
            ShowBackButtonIfCanGoBack();
        }

        bool CanGoBackCompute()
        {
            switch (CurrentView)
            {
                case View.Main:
                    return Loc.Main.TopicVisible;
                case View.Editor:
                    return true;
                default:
                    return _navigationFrame.CanGoBack;
            }            
        }

        public void Navigate(View page, object parameter)
        {
            switch (page)
            {
                case View.Connect:
                    _navigationFrame.Navigate(typeof(ConnectPage));
                    break;
                case View.Main:
                    _navigationFrame.Navigate(typeof(MainPage));
                    break;
                case View.Editor:
                    _navigationFrame.Navigate(typeof(EditorPage), parameter);
                    break;
                case View.Settings:
                    _navigationFrame.Navigate(typeof (Settings));
                    break;
                default:
                    break;
            }
            CurrentView = page;
            ShowBackButtonIfCanGoBack();
        }
        public void Navigate(View page)
        {
            Navigate(page, null);
        }

        public Page CurrentPage { get { return _navigationFrame.Content as Page; } }
    }
}
