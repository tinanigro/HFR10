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
                Task.Run(() => TopicFetcher.GetPosts(Loc.Topic.CurrentTopic));
            }
        }
    }
}