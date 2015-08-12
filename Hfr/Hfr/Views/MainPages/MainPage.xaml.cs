using Hfr.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Hfr.Views.MainPages
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Window.Current.SizeChanged += CurrentOnSizeChanged;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Window.Current.SizeChanged -= CurrentOnSizeChanged;
        }

        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            if (windowSizeChangedEventArgs.Size.Width < 1050)
            {
                VisualStateManager.GoToState(this, Loc.Main.Topics.Any() ? "SnapTopicView" : "SnapTopicList", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "FullTwoColumns", false);
            }
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "SnapTopicList", false);
        }

        private void Drapeaux_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Window.Current.Bounds.Width < 1050)
            {
                VisualStateManager.GoToState(this, "SnapTopicView", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "FullTwoColumns", false);
            }
        }
        private void SemanticZoom_OnViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            DrapsZoomeOutListView.ItemsSource = DrapsCvs.View.CollectionGroups;
        }
    }
}
