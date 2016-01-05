using System.Linq;
#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using Data = System.Windows.Data;
#endif

#if WINDOWS_UAP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
#endif

namespace Huyn.MultiBinding
{
    public class MultiBindings : Panel
    {

        public void Init(FrameworkElement targetItem)
        {

            //set the datacontext
            DataContext = targetItem.DataContext;
#if WINDOWS_UAP
            targetItem.DataContextChanged += (sender, e) =>
            {
                DataContext = e.NewValue;
            };
#endif
#if SILVERLIGHT
            DataContextWatcher.Watch(targetItem, (e) =>
            {
                DataContext = e.NewValue;
            });
#endif


            foreach (var multiBinding in Children.OfType<MultiBinding>())
            {
                multiBinding.Init(targetItem);
            }
        }
    }
}