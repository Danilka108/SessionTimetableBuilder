using System;
using System.Globalization;
using Adapters;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Infrastructure;

public class LocalizedMessageConverter : IValueConverter, ILocalizedMessageConverter
{
    public string Convert(LocalizedMessage message)
    {
        var (resourceKey, formatArgs) = GetMessageResourceKey(message);
        return ConvertByResourceKey(resourceKey, formatArgs);
    }

    public string Convert(LocalizedMessage.Header header)
    {
        var resourceKey = GetHeaderResourceKey(header);
        return ConvertByResourceKey(resourceKey, new object?[] { });
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var (resourceKey, formatArgs) = value switch
        {
            LocalizedMessage message => GetMessageResourceKey(message),
            LocalizedMessage.Header header => (GetHeaderResourceKey(header), new object?[] { }),
            string key => (key, new object?[] { }),
            _ => throw new ConvertLocalizedMessageException("Undefined input value type")
        };

        return ConvertByResourceKey(resourceKey, formatArgs);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private string GetHeaderResourceKey(LocalizedMessage.Header header)
    {
        return header switch
        {
            LocalizedMessage.Header.Delete => "DeleteDialogHeader",
            LocalizedMessage.Header.Error => "ErrorDialogHeader"
        };
    }

    private (string, object?[]) GetMessageResourceKey(LocalizedMessage message)
    {
        return message switch
        {
            LocalizedMessage.Error.UndefinedError => ("CreateClassroomFeatureError",
                new object?[] { }),
            LocalizedMessage.Error.StorageIsNotAvailable =>
                ("StorageIsNotAvailableError", new object?[] { }),
            LocalizedMessage.Error.ClassroomFeatureReferencedByClassroom v =>
                ("ClassroomFeatureReferencedByClassroom", new object?[] { v.ClassroomNumber }),
            LocalizedMessage.Error.ClassroomFeatureReferencedByDiscipline v =>
                ("ClassroomFeatureReferencedByDiscipline", new object?[] { v.DisciplineName }),
            LocalizedMessage.Error.DescriptionOfClassroomFeatureMustBeOriginal =>
                ("DescriptionOfClassroomFeatureMustMeOriginalError", new object?[] { }),
            LocalizedMessage.Error.NumberOfClassroomMustBeOriginal => (
                "NumberOfClassroomMustBeOriginal", new object?[] { }),
            LocalizedMessage.Error.NameOfDisciplineMustBeOriginal => (
                "NameOfDisciplineMustBeOriginalError", new object?[] { }),
            LocalizedMessage.FieldError.InvalidNumericString => (
                "InvalidNumericStringFieldError", new object?[] { }),
            LocalizedMessage.FieldError.CantBeEmpty => ("CantBeEmptyFieldError", new object?[] { }),
            LocalizedMessage.FieldError.Separator => ("FieldErrorSeparator", new object?[] { }),
            LocalizedMessage.Question.DeleteClassroomFeature => ("DeleteClassroomFeatureQuestion",
                new object?[] { }),
            LocalizedMessage.Question.DeleteClassroom => ("DeleteClassroomQuestion",
                new object?[] { })
        };
    }

    private string ConvertByResourceKey(string resourceKey, object?[] args)
    {
        if (Avalonia.Application.Current is null)
            throw new ConvertLocalizedMessageException(
                "Could not get current application instance");

        Avalonia.Application.Current.TryFindResource(resourceKey, out var resourceValue);

        if (resourceValue is not string stringResourceValue)
            throw new ConvertLocalizedMessageException("Could not find string resource");

        try
        {
            return string.Format(stringResourceValue, args);
        }
        catch (Exception e)
        {
            throw new ConvertLocalizedMessageException("Could not apply string format", e);
        }
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