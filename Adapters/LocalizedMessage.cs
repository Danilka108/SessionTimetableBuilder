namespace Adapters;

public abstract record LocalizedMessage
{
    private LocalizedMessage()
    {
    }

    public enum Header
    {
        Delete,
        Error
    }

    public static class Error
    {
        public record UndefinedError : LocalizedMessage;

        public record StorageIsNotAvailable : LocalizedMessage;

        public record DescriptionOfClassroomFeatureMustBeOriginal : LocalizedMessage;

        public record ClassroomFeatureAlreadyLinkedByClassroom
            (int ClassroomNumber) : LocalizedMessage;
        
        public record NumberOfClassroomMustBeOriginal : LocalizedMessage;
    }

    public static class FieldError
    {
        public record InvalidNumericString : LocalizedMessage;
        
        public record CantBeEmpty : LocalizedMessage;

        public record Separator : LocalizedMessage;
    }

    public static class Question
    {
        public record DeleteClassroomFeature : LocalizedMessage;

        public record DeleteClassroom : LocalizedMessage;
    }
}