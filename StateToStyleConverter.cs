using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using inline.Model;

namespace inline
{
    public class StateToStyleConverter : IValueConverter
    {
        private readonly Style [] _styles;
        public StateToStyleConverter(Style[] styles)
        {
            if (styles == null) throw new ArgumentNullException("styles");
            _styles = styles;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            State state = (State)value;
            return _styles[(int)state];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
