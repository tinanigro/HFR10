using Windows.UI.Xaml.Controls;

namespace Hfr.Views.MainPages.MainPageControls.CategoriesViews
{
    public sealed partial class CategoriesList : Page
    {
        public CategoriesList()
        {
            this.InitializeComponent();
        }
        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            CatsZoomeOutListView.ItemsSource = CatsCvs.View.CollectionGroups;
        }
    }
}
