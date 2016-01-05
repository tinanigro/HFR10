using System;
using System.Globalization;

#if SILVERLIGHT
using System.Windows;
using Data = System.Windows.Data;
#endif

#if WINDOWS_UAP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Data = Windows.UI.Xaml.Data;
#endif

namespace Huyn.MultiBinding
{
    // Facade of a Binding
    public class Binding : FrameworkElement
    {
        public event ComputedValueChangedEventHandler ComputedValueChanged;

        private Data.Binding _generatedBinding;

        public Binding()
        {
            _generatedBinding = new Data.Binding();
        }



        #region ComputedValue

        protected static readonly DependencyProperty ComputedValueProperty =
            DependencyProperty.Register("ComputedValue", typeof(object), typeof(Binding),
                new PropertyMetadata(null, OnComputedValueChanged));
        private FrameworkElement _targetItem;

        protected internal object ComputedValue
        {
            get
            {
                return GetValue(ComputedValueProperty);
            }
            set { SetValue(ComputedValueProperty, ComputedValue); }
        }

        private static void OnComputedValueChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var oneBinding = depObj as Binding;
            if (oneBinding != null)
                oneBinding.ComputedValueChanged(oneBinding, e.NewValue);
        }

        #endregion


        #region Binding properties

        public string Path
        {
            get { return _generatedBinding.Path != null ? _generatedBinding.Path.Path : null; }
            set
            {
                _generatedBinding.Path = new PropertyPath(value);
            }
        }

        public Data.IValueConverter Converter
        {
            get { return _generatedBinding.Converter; }
            set { _generatedBinding.Converter = value; }
        }

        public object ConverterParameter
        {
            get { return _generatedBinding.ConverterParameter; }
            set { _generatedBinding.ConverterParameter = value; }
        }

#if SILVERLIGHT
        public CultureInfo ConverterCulture
        {
            get { return _generatedBinding.ConverterCulture; }
            set { _generatedBinding.ConverterCulture = value; }
        }
#endif
#if WINRT
        public string ConverterLanguage
        {
            get { return _generatedBinding.ConverterLanguage; }
            set { _generatedBinding.ConverterLanguage = value; }
        }
#endif


        public Data.BindingMode Mode
        {
            get { return _generatedBinding.Mode; }
            set { _generatedBinding.Mode = value; }
        }


        public object Source
        {
            get { return _generatedBinding.Source; }
            set { _generatedBinding.Source = ComputedValue; }
        }

        public object TargetNullComputedValue
        {
            get { return _generatedBinding.TargetNullValue; }
            set { _generatedBinding.TargetNullValue = value; }
        }

#if WINRT
        public Data.UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return _generatedBinding.UpdateSourceTrigger; }
            set { _generatedBinding.UpdateSourceTrigger = value; }
        }
#endif

#if SILVERLIGHT

        public string StringFormat
        {
            get { return _generatedBinding.StringFormat; }
            set { _generatedBinding.StringFormat = value; }
        }

        public bool NotifyOnValidationError
        {
            get { return _generatedBinding.NotifyOnValidationError; }
            set { _generatedBinding.NotifyOnValidationError = value; }
        }

        public bool ValidatesOnDataErrors
        {
            get { return _generatedBinding.ValidatesOnDataErrors; }
            set { _generatedBinding.ValidatesOnDataErrors = value; }
        }

        public bool ValidatesOnExceptions
        {
            get { return _generatedBinding.ValidatesOnExceptions; }
            set { _generatedBinding.ValidatesOnExceptions = value; }
        }


        public bool ValidatesOnNotifyDataErrors
        {
            get { return _generatedBinding.ValidatesOnNotifyDataErrors; }
            set { _generatedBinding.ValidatesOnNotifyDataErrors = value; }
        }
#endif

        public object FallbackComputedValue
        {
            get { return _generatedBinding.FallbackValue; }
            set { _generatedBinding.FallbackValue = ComputedValue; }
        }

        public Data.RelativeSource RelativeSource
        {
            get;
            set;
        }

        public string ElementName { get; set; }

        #endregion

        internal void Init(FrameworkElement targetItem)
        {
            //Manage Element Name
            if (!String.IsNullOrEmpty(ElementName))
            {
                _targetItem = targetItem;
                targetItem.LayoutUpdated += TargetItemElementName_LayoutUpdated;
            }
            else if (RelativeSource != null)
            {
                _targetItem = targetItem;
                targetItem.LayoutUpdated += TargetItemRelativeSource_LayoutUpdated;
            }
            else
            {
                SetBinding(ComputedValueProperty, _generatedBinding);
            }
        }

#if WINDOWS_UAP
        private void TargetItemElementName_LayoutUpdated(object sender, object e)
#else
        private void TargetItemElementName_LayoutUpdated(object sender, EventArgs e)
#endif
        {
            _targetItem.LayoutUpdated -= TargetItemElementName_LayoutUpdated;


            var item = _targetItem.FindName(ElementName) as FrameworkElement;
            if (item != null)
            {
                _generatedBinding.Source = item;
            }
            SetBinding(ComputedValueProperty, _generatedBinding);

        }
#if WINDOWS_UAP
        private void TargetItemRelativeSource_LayoutUpdated(object sender, object e)
#else
        private void TargetItemRelativeSource_LayoutUpdated(object sender, EventArgs e)
#endif
        {
            _targetItem.LayoutUpdated -= TargetItemRelativeSource_LayoutUpdated;


            switch (RelativeSource.Mode)
            {
                case Data.RelativeSourceMode.Self:
                    _generatedBinding.Source = _targetItem;
                    break;
            }
            SetBinding(ComputedValueProperty, _generatedBinding);
        }


    }


    public delegate void ComputedValueChangedEventHandler(Binding sender, object ComputedValue);
}