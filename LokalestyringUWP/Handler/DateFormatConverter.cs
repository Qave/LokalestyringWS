using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LokalestyringUWP.Handler
{
    public class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string dt = "";
            if (value == null)
                return null;
            if (value.GetType() == typeof(DateTime))
            {
                dt = DateTime.Parse(value.ToString()).ToString("dd/MM/hh");
            }
            if (value.GetType() == typeof(TimeSpan))
            {
                dt = TimeSpan.Parse(value.ToString()).ToString(@"hh\:mm");
            }
            return dt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
