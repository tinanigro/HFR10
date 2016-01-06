using System.Threading.Tasks;
using Hfr.Helpers;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Topics
{
    public class ChangeTopicsListPageInSubCatCommand : Command
    {
        public override void Execute(object parameter)
        {
            // Don't forget Index starts at 1, not 0, here.
            if (parameter is string)
            {
                var action = (string)parameter;
                if (action == "+")
                {
                    Loc.SubCategory.TopicsPage = Loc.SubCategory.TopicsPage + 1;
                }
                else if (action == "-" && Loc.SubCategory.TopicsPage > 1)
                {
                    Loc.SubCategory.TopicsPage = Loc.SubCategory.TopicsPage - 1;
                }
                Task.Run(() => CatFetcher.GetTopics(Loc.SubCategory.CurrentSubCategory));
            }
        }
    }
}
