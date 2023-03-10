﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitCredentialManager.UI.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class NonEmptyStringToVisibleConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterHelper.GetConditionalVisibility(!string.IsNullOrEmpty(value as string), parameter);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    [ValueConversion(typeof(string), typeof(Visibility))]
    public class NonNullToVisibleConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterHelper.GetConditionalVisibility(value != null, parameter);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
