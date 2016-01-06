using Hfr.ViewModel;
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
            await Loc.Editor.OnNavigatedTo(e.Parameter);
            Loc.Editor.PropertyChanged += Editor_PropertyChanged;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Loc.Editor.OnNavigatedFrom();
            Loc.Editor.PropertyChanged -= Editor_PropertyChanged;
        }

        private void Editor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Loc.Editor.IsEditorEnabled))
            {
                if (Loc.Editor.IsEditorEnabled)
                    MessageTextBlock.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }
    }
}
