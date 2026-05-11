using System.Globalization;
using System.Windows.Data;

namespace WorstAppEver.Converters;

public sealed class BoolToStatusTextConverter : IValueConverter
{
    public string TrueText { get; set; } = "Active";
    public string FalseText { get; set; } = "Idle";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is true ? TrueText : FalseText;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
