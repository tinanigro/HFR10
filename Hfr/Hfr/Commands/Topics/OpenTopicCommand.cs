using System.Linq;
using Windows.UI.Xaml.Controls;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Threading.Tasks;
using Hfr.Helpers;
using System.Collections;
using System.Collections.Generic;

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
                topic = (Model.Topic)parameter;
            }
            else if (parameter is IDictionary)
            {
                var elements = (Dictionary<object, object>)parameter;
                topic = elements["topic"] as Model.Topic;
                var desiredPage = elements["page"]?.ToString();
                var desiredPageValue = 0;
                if (!int.TryParse(desiredPage, out desiredPageValue)) return;
                topic.TopicCurrentPage = desiredPageValue;
            }

            if (topic == null) return;

            if (!Loc.Main.Topics.Any())
                Loc.Main.Topics.Add(topic);
            else Loc.Main.Topics[0] = topic;
            Loc.Topic.SelectedTopic = 0;

            Task.Run(async () => await TopicFetcher.GetPosts(Loc.Topic.CurrentTopic));

            if (Loc.NavigationService.CurrentView != View.Main)
                Loc.NavigationService.Navigate(View.Main);
        }
    }
}
