using Hfr.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Hfr.Views.MainPages
{
    public sealed partial class EditorPage : UserControl
    {
        private string url;
        public EditorPage(object quoteUrl)
        {
            this.InitializeComponent();
            this.url = quoteUrl?.ToString();
            this.Loaded += EditorPage_Loaded;
            this.Unloaded += EditorPage_Unloaded;
        }

        private void EditorPage_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loc.Editor.OnNavigatedFrom();
        }

        private void EditorPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loc.Editor.OnNavigatedTo(url);
        }
    }
}
