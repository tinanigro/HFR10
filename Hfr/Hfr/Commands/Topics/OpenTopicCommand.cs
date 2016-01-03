using System.Linq;
using Windows.UI.Xaml.Controls;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Topics
{
    public class OpenTopicCommand : Command
    {
        public override void Execute(object parameter)
        {
            Model.Topic topic = null;
            if (parameter is ItemClickEventArgs)
            {
                var itemClick = parameter as ItemClickEventArgs;
                topic = itemClick.ClickedItem as Model.Topic;
            }
            else if (parameter is Model.Topic)
            {
                topic = (Model.Topic) parameter;
            }

            if (!Loc.Main.Topics.Any())
                Loc.Main.Topics.Add(topic);
            else Loc.Main.Topics[0] = topic;
            Loc.Topic.SelectedTopic = 0;
        }
    }
}
