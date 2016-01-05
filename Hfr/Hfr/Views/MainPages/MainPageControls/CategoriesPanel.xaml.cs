using System;
using Windows.UI.Xaml.Controls;
using Hfr.Model;
using Hfr.Views.MainPages.MainPageControls.CategoriesViews;

namespace Hfr.Views.MainPages.MainPageControls
{
    public sealed partial class CategoriesPanel : UserControl
    {
        public CategoriesPanel()
        {
            this.InitializeComponent();
            Navigate(View.CategoriesList);
        }

        public void Navigate(View view)
        {
            switch (view)
            {
                case View.CategoriesList:
                    NavigationFrame.Navigate(typeof(CategoriesList));
                    break;
                case View.CategoryTopicsList:
                    NavigationFrame.Navigate(typeof (CategoryTopicsList));
                    break;
            }
        }
    }
}
