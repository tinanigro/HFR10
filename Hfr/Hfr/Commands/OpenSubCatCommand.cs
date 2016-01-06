using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Hfr.Helpers;
using Hfr.Model;
using Hfr.Models;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands
{
    public class OpenSubCatCommand : Command
    {
        public override void Execute(object parameter)
        {
            var itemClick = parameter as ItemClickEventArgs;
            if (itemClick != null)
            {
                var subcat = itemClick.ClickedItem as SubCategory;
                Loc.SubCategory.CurrentSubCategory = subcat;
                Loc.NavigationService.Navigate(View.CategoryTopicsList);
                Task.Run(() => CatFetcher.GetTopics(subcat));
            }
        }
    }
}
