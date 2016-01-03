using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hfr.Views
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            this.InitializeComponent();
        }
        
        public object ExtraPaneContent
        {
            get { return (object)GetValue(ExtraPaneContentProperty); }
            set { SetValue(ExtraPaneContentProperty, value); }
        }

        public static readonly DependencyProperty ExtraPaneContentProperty = DependencyProperty.Register(nameof(ExtraPaneContent), typeof(object), typeof(Shell), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (Shell) dependencyObject;
            that.UpdateExtraPaneContent();
        }

        void UpdateExtraPaneContent()
        {
            ExtraPaneGrid.Visibility = (ExtraPaneContent == null) ? Visibility.Collapsed : Visibility.Visible;
            ExtraPaneContentPresenter.Content = ExtraPaneContent;
        }
    }
}
