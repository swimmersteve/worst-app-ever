using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WorstAppEver.Converters;

public sealed class BoolToColorConverter : IValueConverter
{
    public Brush TrueColor { get; set; } = new SolidColorBrush(Color.FromRgb(0xE7, 0x4C, 0x3C));
    public Brush FalseColor { get; set; } = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is true ? TrueColor : FalseColor;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
