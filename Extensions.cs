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
using System.ComponentModel;

namespace inline
{
    public static class Extensions
    {

        public static void RaisePropertyChanged(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler, string propertyName)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
