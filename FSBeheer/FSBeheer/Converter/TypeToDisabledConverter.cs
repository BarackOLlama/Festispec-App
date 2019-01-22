using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FSBeheer.View
{
    public class TypeToDisabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return value == null ? Visibility.Collapsed : Visibility.Visible;
            return value.Equals("Open Vraag") || value.Equals("Open Tabelvraag") ? false : true; 
                    // if then                                         //else
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
