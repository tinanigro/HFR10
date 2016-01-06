using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hfr.Models;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.UI
{
    public class SetDefaultColumnViewCommand : Command
    {
        public override void Execute(object parameter)
        {
            MainColumn column = MainColumn.TopicsList;
            if (parameter is MainColumn)
            {
                column = (MainColumn) parameter;
            }
            else if (parameter is int)
            {
                column = (MainColumn) parameter;
            }
            else if (parameter is string)
            {
                var index = int.Parse((string) parameter);
                column = (MainColumn) index;
            }
            Loc.Main.DefaultColumn = column;
        }
    }
}
