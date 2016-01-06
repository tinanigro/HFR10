using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hfr.Helpers;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Topic
{
    public class RefreshTopicCommand : Command
    {
        public override void Execute(object parameter)
        {
            Task.Run(async () => await TopicFetcher.GetPosts(Loc.Topic.CurrentTopic));
        }
    }
}
