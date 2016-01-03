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
            var itemClick = parameter as ItemClickEventArgs;
            if (itemClick != null)
            {
                var topic = itemClick.ClickedItem as Model.Topic;
                if (!Loc.Main.Topics.Any())
                    Loc.Main.Topics.Add(topic);
                else Loc.Main.Topics[0] = topic;
                Loc.Topic.SelectedTopic = 0;
            }
        }
    }
}
