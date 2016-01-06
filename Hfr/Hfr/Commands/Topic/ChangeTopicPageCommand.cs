using System.Threading.Tasks;
using Hfr.Helpers;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Topic
{
    public class ChangeTopicPageCommand : Command
    {
        public override void Execute(object parameter)
        {
            // Don't forget Index starts at 1, not 0, here.
            if (parameter is string)
            {
                var action = (string)parameter;
                if (action == "+" && Loc.Topic.CurrentTopic.TopicCurrentPage < Loc.Topic.CurrentTopic.TopicNbPage)
                {
                    Loc.Topic.CurrentTopic.TopicCurrentPage = Loc.Topic.CurrentTopic.TopicCurrentPage + 1;
                }
                else if (action == "-" && Loc.Topic.CurrentTopic.TopicCurrentPage > 1)
                {
                    Loc.Topic.CurrentTopic.TopicCurrentPage = Loc.Topic.CurrentTopic.TopicCurrentPage - 1;
                }
                else if (action == Strings.Last)
                {
                    Loc.Topic.CurrentTopic.TopicCurrentPage = Loc.Topic.CurrentTopic.TopicNbPage;
                }
                else if (action == Strings.First)
                {
                    Loc.Topic.CurrentTopic.TopicCurrentPage = 1;
                }
                else if (!string.IsNullOrEmpty(action))
                {
                    int index = -1;
                    if (!int.TryParse(action, out index)) return;
                    if (index == 0 || index > Loc.Topic.CurrentTopic.TopicNbPage) return;
                    Loc.Topic.CurrentTopic.TopicCurrentPage = index;
                }
                Task.Run(() => TopicFetcher.GetPosts(Loc.Topic.CurrentTopic));
            }
        }
    }
}