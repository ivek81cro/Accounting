using System.Windows;
using System.Windows.Controls;

namespace AccountingUI.Core.Helpers
{
    public class DatePickerHelper
    {
        public static object GetWatermark(DependencyObject obj)
        {
            return (object)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, object value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(object), typeof(DatePickerHelper),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnWatermarkChanged)));

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = d as DatePicker;
            if (datePicker == null)
                return;
            if (datePicker.IsLoaded)
            {
                SetWatermarkInternal(datePicker, e.NewValue);
            }
            else
            {
                RoutedEventHandler loadedHandler = null;
                loadedHandler = delegate
                {
                    datePicker.Loaded -= loadedHandler;
                    SetWatermarkInternal(datePicker, e.NewValue);
                };
                datePicker.Loaded += loadedHandler;
            }
        }
        private static void SetWatermarkInternal(DatePicker d, object value)
        {
            var textBox = d.Template.FindName("PART_TextBox", d) as Control;
            if (textBox != null)
            {
                var watermarkControl = textBox.Template.FindName("PART_Watermark", textBox) as ContentControl;
                if (watermarkControl != null)
                    watermarkControl.Content = value;
            }
        }

    }
}
