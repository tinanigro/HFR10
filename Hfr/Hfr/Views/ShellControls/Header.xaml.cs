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
            UpdateLeftContent();
        }

        #region left content
        public object LeftContent
        {
            get { return (object)GetValue(LeftContentProperty); }
            set { SetValue(LeftContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register(nameof(LeftContent), typeof(object), typeof(Header), new PropertyMetadata(null, LeftContentPropertyChanged));

        private static void LeftContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = dependencyObject as Header;
            that.UpdateLeftContent();
        }

        public bool LeftContentVisible
        {
            get { return (bool)GetValue(LeftContentVisibleProperty); }
            set { SetValue(LeftContentVisibleProperty, value); }
        }

        public static readonly DependencyProperty LeftContentVisibleProperty =
            DependencyProperty.Register(nameof(LeftContentVisible), typeof(bool), typeof(Header), new PropertyMetadata(true, LeftContentVisiblePropertyChanged));

        private static void LeftContentVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = dependencyObject as Header;
            that.UpdateLeftContent();
        }

        private void UpdateLeftContent()
        {
            LeftContentPresenter.Visibility = (LeftContentVisible && LeftContent != null) ? Visibility.Visible : Visibility.Collapsed;
            LeftContentPresenter.Content = LeftContent;
        }
        #endregion

        #region header content
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
        #endregion
        #region right content

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
        #endregion
    }
}
