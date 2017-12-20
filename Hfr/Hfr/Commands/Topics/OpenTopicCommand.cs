using System.Linq;
using Windows.UI.Xaml.Controls;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Threading.Tasks;
using Hfr.Helpers;
using System.Collections;
using System.Collections.Generic;

namespace Hfr.Commands.Threads
{
    public class OpenTopicCommand : Command
    {
        public override void Execute(object parameter)
        {
            Models.Threads.Topic topic = null;
            if (parameter is ItemClickEventArgs)
            {
                var itemClick = parameter as ItemClickEventArgs;
                topic = itemClick.ClickedItem as Models.Threads.Topic;
            }
            else if (parameter is Models.Threads.Topic)
            {
                topic = (Models.Threads.Topic)parameter;
            }
            else if (parameter is IDictionary)
            {
                var elements = (Dictionary<object, object>)parameter;
                topic = elements["topic"] as Models.Threads.Topic;
                var desiredPage = elements["page"]?.ToString();
                var desiredPageValue = 0;
                if (!int.TryParse(desiredPage, out desiredPageValue)) return;
                topic.ThreadCurrentPage = desiredPageValue;
            }

            if (topic == null) return;

            if (!Loc.Main.Threads.Any())
                Loc.Main.Threads.Add(topic);
            else Loc.Main.Threads[0] = topic;
            Loc.Thread.SelectedThread = 0;

            Task.Run(async () => await ThreadFetcher.GetPosts(Loc.Thread.CurrentThread));

            Loc.NavigationService.Navigate(View.ThreadTab);
        }
    }
}
