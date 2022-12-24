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
            (int classroomNumber) : LocalizedMessage;

        public record NumberOfClassroomMustBeOriginal : LocalizedMessage;
    }

    public static class Question
    {
        public record DeleteClassroomFeature : LocalizedMessage;

        public record DeleteClassroom : LocalizedMessage;
    }
}