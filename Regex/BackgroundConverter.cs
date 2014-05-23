using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Regex
{
    public class BorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double isValid = 0;
            double isInvalid = 1;
            if (value == null)
                return isInvalid;
            if (value.GetType() != typeof(bool))
            {
                return isInvalid;
            }

            if((bool)value == true)
            {
                return isValid;
            }
            return isInvalid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
