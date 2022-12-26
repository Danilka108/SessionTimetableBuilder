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

    public string Convert(LocalizedMessage.Letter letter)
    {
        var resourceKey = GetLetterResourceKey(letter);
        return ConvertByResourceKey(resourceKey, new object?[] { });
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var (resourceKey, formatArgs) = value switch
        {
            LocalizedMessage message => GetMessageResourceKey(message),
            LocalizedMessage.Letter letter => (GetLetterResourceKey(letter), new object?[] { }),
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

    private string GetLetterResourceKey(LocalizedMessage.Letter letter)
    {
        return letter switch
        {
            LocalizedMessage.Letter.Delete => "DeleteDialogLetter",
            LocalizedMessage.Letter.Error => "ErrorDialogLetter",
            LocalizedMessage.Letter.Group => "GroupLetter",
            LocalizedMessage.Letter.Lecturer => "LecturerLetter",
            LocalizedMessage.Letter.Close => "CloseLetter",
            LocalizedMessage.Letter.Exam => "ExamLetter"
        };
    }

    private (string, object?[]) GetMessageResourceKey(LocalizedMessage message)
    {
        return message switch
        {
            LocalizedMessage.Error.GroupDoesNotStudyDiscipline v =>
                ("GroupDoesNotStudyDisciplineError", new object?[] {v.GroupName, v.DisciplineName}),
            LocalizedMessage.Error.LecturerDoesNotAcceptDiscipline v =>
                ("LecturerDoesNotAcceptDisciplineError",
                    new object?[] { v.LecturerName, v.DisciplineName }),
            LocalizedMessage.Error.GroupIsNotSelected =>
                ("GroupIsNotSelectedError", new object?[] { }),
            LocalizedMessage.Error.LecturerIsNotSelected =>
                ("LecturerIsNotSelectedError", new object?[] { }),
            LocalizedMessage.Error.DisciplineIsNotSelected =>
                ("DisciplineIsNotSelectedError", new object?[] { }),
            LocalizedMessage.Error.ClassroomIsNotSelected =>
                ("ClassroomIsNotSelectedError", new object?[] { }),
            LocalizedMessage.Error.EnteredDateTimeIsNotValid =>
                ("EnteredDateTimeIsNotValidError", new object?[] { }),
            LocalizedMessage.Error.ClassroomDoesNotMeetsRequirements v =>
                ("ClassroomDoesNotMeetsRequirementsError",
                    new object?[] { v.ClassroomNumber, v.DisciplineName }),
            LocalizedMessage.Error.DisciplineReferencedByExam v =>
                ("DisciplineReferencedByExamError", new object?[] { v.GroupName }),
            LocalizedMessage.Error.GroupReferencedByExam v =>
                ("GroupReferencedByExamError", new object?[] { v.DisciplineName }),
            LocalizedMessage.Error.ClassroomReferencedByExam v =>
                ("ClassroomReferencedByExamError", new object?[] { v.GroupName, v.DisciplineName }),
            LocalizedMessage.Error.LecturerReferencedByExam v =>
                ("LecturerReferencedByExamError", new object?[] { v.GroupName, v.DisciplineName }),
            LocalizedMessage.Error.SameExamAlreadyExists v =>
                ("LecturerReferencedByExamError", new object?[] { v.GroupName, v.DisciplineName }),
            LocalizedMessage.Error.UndefinedError => ("CreateClassroomFeatureError",
                new object?[] { }),
            LocalizedMessage.Error.StorageIsNotAvailable =>
                ("StorageIsNotAvailableError", new object?[] { }),
            LocalizedMessage.Error.ClassroomFeatureReferencedByClassroom v =>
                ("ClassroomFeatureReferencedByClassroom", new object?[] { v.ClassroomNumber }),
            LocalizedMessage.Error.ClassroomFeatureReferencedByDiscipline v =>
                ("ClassroomFeatureReferencedByDiscipline", new object?[] { v.DisciplineName }),
            LocalizedMessage.Error.DisciplineReferencedByLecturer v =>
                ("DisciplineReferencedByLecturer", new object?[] { v.LecturerName }),
            LocalizedMessage.Error.DescriptionOfClassroomFeatureMustBeOriginal =>
                ("DescriptionOfClassroomFeatureMustMeOriginalError", new object?[] { }),
            LocalizedMessage.Error.NumberOfClassroomMustBeOriginal => (
                "NumberOfClassroomMustBeOriginal", new object?[] { }),
            LocalizedMessage.Error.NameOfDisciplineMustBeOriginal => (
                "NameOfDisciplineMustBeOriginalError", new object?[] { }),
            LocalizedMessage.Error.DisciplineReferencedByGroup v => (
                "DisciplineReferencedByGroupError", new object?[] { v.GroupName }),
            LocalizedMessage.Error.NameOfGroupMustBeOriginal => (
                "NameOfGroupMustBeOriginalError", new object?[] { }),
            LocalizedMessage.FieldError.InvalidNumericString => (
                "InvalidNumericStringFieldError", new object?[] { }),
            LocalizedMessage.FieldError.CantBeEmpty => ("CantBeEmptyFieldError", new object?[] { }),
            LocalizedMessage.FieldError.Separator => ("FieldErrorSeparator", new object?[] { }),
            LocalizedMessage.Question.DeleteClassroomFeature => ("DeleteClassroomFeatureQuestion",
                new object?[] { }),
            LocalizedMessage.Question.DeleteClassroom => ("DeleteClassroomQuestion",
                new object?[] { }),
            LocalizedMessage.Question.DeleteDiscipline => ("DeleteDisciplineQuestion",
                new object?[] { }),
            LocalizedMessage.Question.DeleteLecturer => ("DeleteLecturerQuestion",
                new object?[] { }),
            LocalizedMessage.Question.DeleteGroup => ("DeleteGroupQuestion", new object?[] { }),
            LocalizedMessage.Question.CloseGroupEditor => ("CloseGroupEditorQuestion",
                new object?[] { }),
            LocalizedMessage.Question.CloseLecturerEditor => ("CloseLecturerEditorQuestion",
                new object?[] { }),
            LocalizedMessage.Question.CloseExamEditor => ("CloseExamEditorQuestion",
                new object?[] { }),
            LocalizedMessage.Question.DeleteExam => ("DeleteExamQuestion", new object?[] { })
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