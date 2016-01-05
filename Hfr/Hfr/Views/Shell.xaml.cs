using Hfr.Views.MiscPages;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hfr.Views
{
    public sealed partial class Shell : UserControl
    {
        private readonly double ExtraPaneDefaultHeight = 750;
        private readonly double ExtraPaneDefaultWidth = 450;
        public Shell()
        {
            this.InitializeComponent();
            this.SizeChanged += Shell_SizeChanged;
        }

        private void Shell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Responsive();
        }

        void Responsive()
        {
            if (this.ActualHeight < ExtraPaneDefaultHeight || this.ActualWidth < ExtraPaneDefaultWidth)
            {
                if (ExtraPaneVisible)
                {
                    ExtraPageFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ExtraPageFrame.VerticalAlignment = VerticalAlignment.Stretch;
                    ExtraPageFrame.Height = this.ActualHeight;
                    ExtraPageFrame.Width = this.ActualWidth;
                }
            }
            else
            {
                if (ExtraPaneVisible)
                {
                    ExtraPageFrame.HorizontalAlignment = HorizontalAlignment.Right;
                    ExtraPageFrame.VerticalAlignment = VerticalAlignment.Bottom;
                    ExtraPageFrame.Height = ExtraPaneDefaultHeight;
                    ExtraPageFrame.Width = ExtraPaneDefaultWidth;
                }
            }
        }

        public bool ExtraPaneVisible => ExtraPageFrame.Content != null;

        public void NavigateExtraFrame(Type type, object parameter)
        {
            ExtraPaneGrid.Visibility = Visibility.Visible;
            ExtraPageFrame.Navigate(type, parameter);
            Responsive();
        }

        public void HideExtraFrame()
        {
            ExtraPaneGrid.Visibility = Visibility.Collapsed;
            ExtraPageFrame.Navigate(typeof(BlankPage));
        }
    }
}
