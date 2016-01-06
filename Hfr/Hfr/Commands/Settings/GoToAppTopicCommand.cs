using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Settings
{
    public class GoToAppTopicCommand : Command
    {
        public override void Execute(object parameter)
        {
            var topic = new Model.Topic();
            topic.TopicCatId = 23;
            topic.TopicDrapURI = "/hfr/gsmgpspda/tablette/tablettes-telephones-windows-sujet_26848_1.htm";
            topic.TopicName = "[Topic Unique] HFR10";
            //topic.TopicDrapURI = 
            Loc.Main.OpenTopicCommand.Execute(topic);
        }
    }
}
