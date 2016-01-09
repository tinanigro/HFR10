using System.Threading.Tasks;
using Hfr.Helpers;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Threads
{
    public class ChangeThreadsListPageInSubCatCommand : Command
    {
        public override void Execute(object parameter)
        {
            // Don't forget Index starts at 1, not 0, here.
            if (parameter is string)
            {
                var action = (string)parameter;
                if (action == "+")
                {
                    Loc.SubCategory.ThreadsPage = Loc.SubCategory.ThreadsPage + 1;
                }
                else if (action == "-" && Loc.SubCategory.ThreadsPage > 1)
                {
                    Loc.SubCategory.ThreadsPage = Loc.SubCategory.ThreadsPage - 1;
                }
                Task.Run(() => CatFetcher.GetThreads(Loc.SubCategory.CurrentSubCategory));
            }
        }
    }
}
