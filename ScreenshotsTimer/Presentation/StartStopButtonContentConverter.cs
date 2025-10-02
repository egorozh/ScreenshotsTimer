using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace ScreenshotsTimer.Presentation;

public class StartStopButtonContentConverter : MarkupExtension, IMultiValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is [bool isRunning, bool isPaused])
        {
            return isRunning
                ? isPaused ? "Start" : "Stop"
                : "Start";
        }

        return "";
    }
}