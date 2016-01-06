using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Hfr.Views.MainPages
{
    public sealed partial class ConnectPage : Page
    {
        public ConnectPage()
        {
            this.InitializeComponent();
        }

        private void PseudoTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                PasswordTextBox.Focus(FocusState.Keyboard);
            }
        }

        private void PseudoTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            PseudoTextBox.Focus(FocusState.Keyboard);
        }
    }
}
