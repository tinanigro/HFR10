using System;
using Hfr.ViewModel;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Hfr.Views.MainPages
{
    public sealed partial class EditorPage : Page
    {
        public EditorPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Loc.Editor.PropertyChanged += Editor_PropertyChanged;
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Loc.Editor.PropertyChanged -= Editor_PropertyChanged;
            Loc.Editor.OnNavigatedFrom();
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = false;
        }

        private void Editor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Loc.Editor.IsEditorEnabled))
            {
                if (Loc.Editor.IsEditorEnabled)
                {
                    MessageTextBlock.IsEnabled = true;
                    MessageTextBlock.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                }
                else
                {
                    MessageTextBlock.IsEnabled = false;
                }
            }
        }

        private async void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var md = new MessageDialog("Ce sera pour la prochaine fois, peut-être :o", "Hé ba non!");
            await md.ShowAsync();
        }
    }
}
