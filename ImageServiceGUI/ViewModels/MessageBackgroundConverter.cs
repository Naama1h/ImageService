using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ImageService.Enums;

namespace ImageServiceGUI.ViewModels
{
    class MessageBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            MessageTypeEnum message = (MessageTypeEnum)value;
            if (message == ImageService.Enums.MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            } else if (message == ImageService.Enums.MessageTypeEnum.INFO)
            {
                return Brushes.LightGreen;
            } else
            {
                return Brushes.Yellow;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
