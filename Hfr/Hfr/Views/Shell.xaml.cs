using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hfr.Views
{
    public sealed partial class Shell : UserControl
    {
        private readonly double ExtraPaneDefaultHeight = 650;
        private readonly double ExtraPaneDefaultWidth = 400;
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
                    ExtraPaneContentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ExtraPaneContentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
                    ExtraPaneContentPresenter.Height = this.ActualHeight;
                    ExtraPaneContentPresenter.Width = this.ActualWidth;
                }
            }
            else
            {
                if (ExtraPaneVisible)
                {
                    ExtraPaneContentPresenter.HorizontalAlignment = HorizontalAlignment.Right;
                    ExtraPaneContentPresenter.VerticalAlignment = VerticalAlignment.Bottom;
                    ExtraPaneContentPresenter.Height = ExtraPaneDefaultHeight;
                    ExtraPaneContentPresenter.Width = ExtraPaneDefaultWidth;
                }
            }
        }

        public bool ExtraPaneVisible => ExtraPaneContent != null;

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
            ExtraPaneGrid.Visibility = (ExtraPaneVisible) ? Visibility.Visible : Visibility.Collapsed;
            ExtraPaneContentPresenter.Content = ExtraPaneContent;
            Responsive();
        }
    }
}
