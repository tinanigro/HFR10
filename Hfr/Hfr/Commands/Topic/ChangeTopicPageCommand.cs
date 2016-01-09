using System.Threading.Tasks;
using Hfr.Helpers;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Topic
{
    public class ChangeThreadPageCommand : Command
    {
        public override void Execute(object parameter)
        {
            // Don't forget Index starts at 1, not 0, here.
            if (parameter is string)
            {
                var action = (string)parameter;
                if (action == "+" && Loc.Thread.CurrentThread.ThreadCurrentPage < Loc.Thread.CurrentThread.ThreadNbPage)
                {
                    Loc.Thread.CurrentThread.ThreadCurrentPage = Loc.Thread.CurrentThread.ThreadCurrentPage + 1;
                }
                else if (action == "-" && Loc.Thread.CurrentThread.ThreadCurrentPage > 1)
                {
                    Loc.Thread.CurrentThread.ThreadCurrentPage = Loc.Thread.CurrentThread.ThreadCurrentPage - 1;
                }
                else if (action == Strings.Last)
                {
                    Loc.Thread.CurrentThread.ThreadCurrentPage = Loc.Thread.CurrentThread.ThreadNbPage;
                }
                else if (action == Strings.First)
                {
                    Loc.Thread.CurrentThread.ThreadCurrentPage = 1;
                }
                else if (!string.IsNullOrEmpty(action))
                {
                    int index = -1;
                    if (!int.TryParse(action, out index)) return;
                    if (index == 0 || index > Loc.Thread.CurrentThread.ThreadNbPage) return;
                    Loc.Thread.CurrentThread.ThreadCurrentPage = index;
                }
                Task.Run(() => ThreadFetcher.GetPosts(Loc.Thread.CurrentThread));
            }
        }
    }
}