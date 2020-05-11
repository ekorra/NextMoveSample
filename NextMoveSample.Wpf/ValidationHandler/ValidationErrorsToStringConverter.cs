using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace NextMoveSample.Wpf.ValidationHandler
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ValidationErrorsToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorList = value as IEnumerable<ValidationError>;
            if (errorList == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            foreach (var error in errorList)
            {
                if (error.ErrorContent != null)
                {
                    sb.AppendLine(error.ErrorContent.ToString());
                }
            }

            var result = sb.ToString().Trim();
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
