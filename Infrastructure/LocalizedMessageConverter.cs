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
        var (resourceKey, formatArgs) = value switch
        {
            LocalizedMessage message => GetMessageResourceKey(message),
            LocalizedMessage.Header header => (GetHeaderResourceKey(header), new object?[] { }),
            string key => (key, new object?[] { }),
            _ => throw new ConvertLocalizedMessageException("Undefined input value type"),
        };

        if (Avalonia.Application.Current is null)
            throw new ConvertLocalizedMessageException(
                "Could not get current application instance");

        Avalonia.Application.Current.TryFindResource(resourceKey, out var resourceValue);

        if (resourceValue is not string stringResourceValue)
            throw new ConvertLocalizedMessageException("Could not find string resource");

        try
        {
            return string.Format(stringResourceValue, formatArgs);
        }
        catch (Exception e)
        {
            throw new ConvertLocalizedMessageException("Could not apply string format", e);
        }
    }

    private string GetHeaderResourceKey(LocalizedMessage.Header header) => header switch
    {
        LocalizedMessage.Header.Delete => "DeleteDialogHeader",
        LocalizedMessage.Header.Error => "ErrorDialogHeader",
    };

    private (string, object?[]) GetMessageResourceKey(LocalizedMessage message) => message switch
    {
        LocalizedMessage.Error.UndefinedError => ("CreateClassroomFeatureError", new object?[] { }),
        LocalizedMessage.Error.StorageIsNotAvailable =>
            ("StorageIsNotAvailableError", new object?[] { }),
        LocalizedMessage.Error.ClassroomFeatureAlreadyLinkedByClassroom v =>
            ("ClassroomFeatureAlreadyLinkedByClassroom", new object?[] { v.classroomNumber }),
        LocalizedMessage.Error.DescriptionOfClassroomFeatureMustBeOriginal =>
            ("DescriptionOfClassroomFeatureMustMeOriginalError", new object?[] { }),
        LocalizedMessage.Error.NumberOfClassroomMustBeOriginal => (
            "NumberOfClassroomMustBeOriginal", new object?[] { }),
        LocalizedMessage.Question.DeleteClassroomFeature => ("DeleteClassroomFeatureQuestion",
            new object?[] { }),
        LocalizedMessage.Question.DeleteClassroom => ("DeleteClassroomQuestion", new object?[] { })
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