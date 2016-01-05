using System;
using System.Diagnostics;
using Hfr.ViewModel;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Hfr.Model;

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
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        public void Navigate(View view)
        {
            switch (view)
            {
                case View.CategoryTopicsList:
                case View.CategoriesList:
                    CategoriesPanel.Navigate(view);
                    break;
            }
        }
    }
}
