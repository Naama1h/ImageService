using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ImageServiceCommunication.Enums;

namespace ImageServiceGUI.ViewModels
{
    class MessageBackgroundConverter : IValueConverter
    {
        /// <summary>
        /// Convert To Color
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="targetType">targetType</param>
        /// <param name="parameter">parameter</param>
        /// <param name="culture">culture</param>
        /// <returns>The Color</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            MessageTypeEnum message = (MessageTypeEnum)value;
            if (message == MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            } else if (message == MessageTypeEnum.INFO)
            {
                return Brushes.LightGreen;
            } else
            {
                return Brushes.Yellow;
            }
        }

        /// <summary>
        /// Convert Back
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="targetType">targetType</param>
        /// <param name="parameter">parameter</param>
        /// <param name="culture">culture</param>
        /// <returns>The Convert Back Object</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
