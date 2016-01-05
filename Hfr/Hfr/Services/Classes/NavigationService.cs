using Windows.UI.Xaml.Controls;
using Hfr.Views.MainPages;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Hfr.Commands.UI;
using Hfr.ViewModel;
using Hfr.Model;
using Hfr.Views;
using Hfr.Views.MiscPages;

namespace Hfr.Services.Classes
{
    public class NavigationService
    {
        private Shell _shell => App.AppShell;
        private Frame _navigationFrame => App.NavigationFrame;
        public View CurrentView;
        public bool CanGoBack
        {
            get { return CanGoBackCompute(); }
        }

        public void Initialize()
        {
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
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = false;
            switch (CurrentView)
            {
                case View.Main:
                    if (Loc.Topic.TopicVisible)
                    {
                        Loc.Topic.SelectedTopic = -1;
                    }
                    break;
                case View.Editor:
                    _shell.HideExtraFrame();
                    CurrentView = View.Main;
                    break;
                case View.Settings:
                    _navigationFrame.GoBack();
                    CurrentView = View.Main;
                    break;
                case View.CategoryTopicsList:
                    ((MainPage)App.NavigationFrame.Content)?.Navigate(View.CategoriesList);
                    CurrentView = View.CategoriesList;
                    break;
                case View.CategoriesList:
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
                    return Loc.Topic.TopicVisible;
                case View.Editor:
                case View.CategoryTopicsList:
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
                    if (CurrentView == View.CategoryTopicsList)
                    {

                    }
                    else
                    {
                        _navigationFrame.Navigate(typeof(MainPage));
                    }
                    break;
                case View.Editor:
                    _shell.NavigateExtraFrame(typeof(EditorPage), parameter);
                    break;
                case View.Settings:
                    _navigationFrame.Navigate(typeof(Settings));
                    break;
                case View.CategoryTopicsList:
                case View.CategoriesList:
                    ((MainPage)App.NavigationFrame.Content)?.Navigate(page);
                    break;
                default:
                    break;
            }
            CurrentView = page;
            ShowBackButtonIfCanGoBack();
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = false;
        }

        public void Navigate(View page)
        {
            Navigate(page, null);
        }

        public Page CurrentPage { get { return _navigationFrame.Content as Page; } }
        public GoBackCommand GoBackCommand { get; } = new GoBackCommand();
        public NavigateToCategoriesList NavigateToCategoriesList { get; } = new NavigateToCategoriesList();
    }
}
