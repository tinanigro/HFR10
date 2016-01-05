using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Hfr.Views.Controls
{
    public sealed partial class StandardTopicItemTemplate : UserControl
    {
        public StandardTopicItemTemplate()
        {
            this.InitializeComponent();
        }

        void RightTapped_Grid(object sender, RightTappedRoutedEventArgs args)
        {
            Flyout.ShowAttachedFlyout((Grid)sender);
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            Flyout.ShowAttachedFlyout((Grid)sender);
        }
    }
}
