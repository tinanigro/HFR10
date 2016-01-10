using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hfr.Views.ShellControls
{
    public sealed partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            this.InitializeComponent();
            this.Loaded += LoadingControl_Loaded;
        }

        private void LoadingControl_Loaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            FadeOut.Completed += FadeOut_Completed;
        }

        private void FadeOut_Completed(object sender, object e)
        {
            Visibility = Visibility.Collapsed;
        }

        public Visibility IsScreenActive
        {
            get { return (Visibility)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsScreenActive), typeof(Visibility), typeof(LoadingControl), new PropertyMetadata(Visibility.Collapsed, PropertyChangedCallback));

        
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (LoadingControl) dependencyObject;
            that.SetActive();
        }

        void SetActive()
        {
            if (IsScreenActive == Visibility.Visible)
            {
                Visibility = Visibility.Visible;
                FadeIn.Begin();
            }
            else
            {
                FadeOut.Begin();
            }
        }

        public string LoadingMessage
        {
            get { return (string)GetValue(LoadingMessageProperty); }
            set { SetValue(LoadingMessageProperty, value); }
        }
        
        public static readonly DependencyProperty LoadingMessageProperty =
            DependencyProperty.Register(nameof(LoadingMessage), typeof(string), typeof(LoadingControl), new PropertyMetadata(null, StringPropertyChangedCallback));

        private static void StringPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (LoadingControl) dependencyObject;
            that.SetString();
        }

        void SetString()
        {
            MessageTextBlock.Text = LoadingMessage;
        }
    }
}
