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

        public record FailedToCreateClassroomFeature : LocalizedMessage;

        public record FailedToUpdateClassroomFeature : LocalizedMessage;

        public record FailedToDeleteClassroomFeature : LocalizedMessage;

        public record DescriptionOfClassroomFeatureMustBeOriginal : LocalizedMessage;
    }

    public static class Question
    {
        public record DeleteCalssroomFeature : LocalizedMessage;
    }
}