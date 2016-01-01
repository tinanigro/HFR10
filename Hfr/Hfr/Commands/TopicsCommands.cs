using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Hfr.Commands
{
    public class OpenTopicCommand : Command
    {
        public override void Execute(object parameter)
        {
            var itemClick = parameter as ItemClickEventArgs;
            if (itemClick != null)
            {
                var topic = itemClick.ClickedItem as Topic;
                if (!Loc.Main.Topics.Any())
                    Loc.Main.Topics.Add(topic);
                else Loc.Main.Topics[0] = topic;
                Loc.Main.SelectedTopic = 0;
            }
        }
    }

    public class RefreshDrapsCommand : Command
    {
        public override void Execute(object parameter)
        {
            Loc.Main.RefreshDraps();
        }
    }

    
}