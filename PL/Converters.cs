using DO;
using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL
{
    public sealed class LattitudeToDmsConverter : IValueConverter
    {
        /// <summary>
        /// converts lattitude coordinate from double type to string of DMS form of the coordinate 
        /// </summary>
        /// <returns> string </returns>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double latitude = (double)value;
                return SexagesimalAngle.FromDouble(latitude);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class LongtitudeToDmsConverter : IValueConverter
    {
        /// <summary>
        /// converts longtitude coordinate from double type to string of DMS form of the coordinate 
        /// </summary>
        /// <returns> string </returns>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double longitude = (double)value;
                return SexagesimalAngle.FromDouble(longitude);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
