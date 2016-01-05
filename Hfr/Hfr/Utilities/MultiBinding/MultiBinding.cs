using System;
using System.Globalization;
using System.Linq;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using Data=System.Windows.Data;
#endif

#if WINDOWS_UAP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Data = Windows.UI.Xaml.Data;
#endif

namespace Huyn.MultiBinding
{
    public class MultiBinding : Panel
    {


        public MultiBinding()
        {
#if WINDOWS_PHONE
            ConverterCulture = CultureInfo.CurrentCulture;
#endif
        }

        #region GeneratedValue

        public static readonly DependencyProperty GeneratedValueProperty =
            DependencyProperty.Register("GeneratedValue", typeof(object), typeof(MultiBinding),
                new PropertyMetadata(null));

        public object GeneratedValue
        {
            get { return GetValue(GeneratedValueProperty); }
            set { SetValue(GeneratedValueProperty, value); }
        }

        #endregion

        #region Converter Parameters

        public string TargetProperty { get; set; }

        public IMultiValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }

#if WINDOWS_UAP
        public string ConverterLanguage { get; set; }
#else
        public CultureInfo ConverterCulture { get; set; }
#endif
        #endregion

        protected internal void Init(FrameworkElement targetElement)
        {
            if (TargetProperty == null)
                throw new Exception("MultiBinding::TargetProperty is not set");
#if WINDOWS_UAP
            var binding = new Data.Binding() { Path = new PropertyPath(TargetProperty), Mode = Data.BindingMode.TwoWay, Source = targetElement };
#else
            var binding = new Data.Binding(TargetProperty) { Mode = Data.BindingMode.TwoWay, Source = targetElement };
#endif
            SetBinding(GeneratedValueProperty, binding);


            foreach (var item in Children.OfType<Binding>())
            {
                item.Init(targetElement);
                item.ComputedValueChanged += InnerBinding_ValueChanged;
            }
        }


        /// One of the binding children has changed, need to update the value
        private void InnerBinding_ValueChanged(Binding sender, object value)
        {
#if WINDOWS_UAP
            if (Converter != null)
            {
                GeneratedValue = Converter.Convert(Children.OfType<Binding>().Select(c => c.ComputedValue).ToArray(),
                    typeof(object), ConverterParameter, ConverterLanguage);
            }
            else
            {
                GeneratedValue = Children?.OfType<Binding>().Select(c => c).ToDictionary(v => v.Tag, v => v.ComputedValue);
            }
#else
            GeneratedValue = Converter.Convert(Children.OfType<Binding>().Select(c => c.ComputedValue).ToArray(),
                typeof(object), ConverterParameter, ConverterCulture);

#endif
        }

    }
}