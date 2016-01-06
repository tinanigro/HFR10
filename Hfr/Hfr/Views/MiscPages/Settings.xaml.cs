using Hfr.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Hfr.Views.MiscPages
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Loc.Settings.OnNavigatedTo();
        }
    }
}
