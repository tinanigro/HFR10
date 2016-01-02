using System;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Hfr.Utilities;

namespace Hfr.Views.ShellControls
{
    public sealed partial class Header : UserControl
    {
        public Header()
        {
            this.InitializeComponent();
            UpdateHamburger();
        }
        
        public bool HamburgerVisible
        {
            get { return (bool)GetValue(HamburgerVisibleProperty); }
            set { SetValue(HamburgerVisibleProperty, value); }
        }
        
        public static readonly DependencyProperty HamburgerVisibleProperty =
            DependencyProperty.Register(nameof(HamburgerVisible), typeof(bool), typeof(Header), new PropertyMetadata(false, HamburgerVisiblePropertyChanged));

        private static void HamburgerVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = dependencyObject as Header;
            that.UpdateHamburger();
        }

        public string HeaderContent
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(string), typeof(Header), new PropertyMetadata("Header", HeaderPropertyChanged));

        static void HeaderPropertyChanged(DependencyObject dpobject, DependencyPropertyChangedEventArgs args)
        {
            var that = dpobject as Header;
            that.Init();
        }

        void Init()
        {
            HeaderTextBlock.Text = HeaderContent.ToUpper();
        }

        void UpdateHamburger()
        {
            HamburgerButtonColumn.Width = (HamburgerVisible) ? new GridLength() : Strings.DefaultMargin;
            HamburgerButton.Visibility = (HamburgerVisible) ? Visibility.Visible : Visibility.Collapsed;
        }



        public object RightContent
        {
            get { return (object)GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(Header), new PropertyMetadata(null, RightContentPropertyChanged));

        private static void RightContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = dependencyObject as Header;
            that.UpdateRightContent();
        }

        void UpdateRightContent()
        {
            RightContentPresenter.Content = RightContent;
        }
    }
}
