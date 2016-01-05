#if WINDOWS_PHONE
using System.Windows;
#endif

#if WINDOWS_UAP
using Windows.UI.Xaml;
#endif

namespace Huyn.MultiBinding
{
    /// Attached property to link MultiBindings to a framework element
    public class MultiBindingLinker
    {
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
                typeof(MultiBindings), typeof(MultiBindingLinker), new PropertyMetadata(null, OnAttachCallback));

        public static MultiBindings GetAttach(DependencyObject obj)
        {
            return (MultiBindings)obj.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject obj, MultiBindings value)
        {
            obj.SetValue(AttachProperty, value);
        }

        private static void OnAttachCallback(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var targetElement = depObj as FrameworkElement;
            var bindings = GetAttach(targetElement);
            bindings.Init(targetElement);
        }
    }
}