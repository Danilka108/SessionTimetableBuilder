using System;
using System.Globalization;
using Adapters;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Infrastructure;

public class LocalizedMessageConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var resourceKey = value switch
        {
            LocalizedMessage message => GetMessageResourceKey(message),
            LocalizedMessage.Header header => GetHeaderResourceKey(header),
            string key => key,
            _ => throw new ConvertLocalizedMessageException("Undefined input value type"),
        };

        if (Avalonia.Application.Current is null)
            throw new ConvertLocalizedMessageException(
                "Could not get current application instance");

        Avalonia.Application.Current.TryFindResource(resourceKey, out var resourceValue);

        if (resourceValue is null)
            throw new ConvertLocalizedMessageException("Could not find resource");

        return resourceValue;
    }

    private string GetHeaderResourceKey(LocalizedMessage.Header header) => header switch
    {
        LocalizedMessage.Header.Delete => "DeleteDialogHeader",
        LocalizedMessage.Header.Error => "ErrorDialogHeader",
    };

    private string GetMessageResourceKey(LocalizedMessage message) => message switch
    {
        LocalizedMessage.Error.UndefinedError => "CreateClassroomFeatureError",
        LocalizedMessage.Error.FailedToCreateClassroomFeature =>
            "CreateClassroomFeatureError",
        LocalizedMessage.Error.FailedToUpdateClassroomFeature =>
            "UpdateClassroomFeatureError",
        LocalizedMessage.Error.FailedToDeleteClassroomFeature =>
            "DeleteClassroomFeatureError",
        LocalizedMessage.Error.DescriptionOfClassroomFeatureMustBeOriginal =>
            "DescriptionOfClassroomFeatureMustMeOriginalError",
        LocalizedMessage.Question.DeleteCalssroomFeature => "DeleteClassroomFeatureQuestion"
    };

    public object ConvertBack(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class ConvertLocalizedMessageException : Exception
{
    public ConvertLocalizedMessageException(string msg) : base(msg)
    {
    }

    public ConvertLocalizedMessageException(string msg, Exception innerException) : base(msg,
        innerException)
    {
    }
}