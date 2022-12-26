namespace Adapters;

public abstract record LocalizedMessage
{
    public enum Letter
    {
        Delete,
        Error,
        Group,
        Lecturer,
        Close,
        Exam
    }

    private LocalizedMessage()
    {
    }

    public static class Error
    {
        public record UndefinedError : LocalizedMessage;

        public record StorageIsNotAvailable : LocalizedMessage;

        public record DescriptionOfClassroomFeatureMustBeOriginal : LocalizedMessage;

        public record ClassroomDoesNotMeetsRequirements(int ClassroomNumber, string DisciplineName) : LocalizedMessage;

        public record ClassroomFeatureReferencedByClassroom
            (int ClassroomNumber) : LocalizedMessage;

        public record ClassroomFeatureReferencedByDiscipline
            (string DisciplineName) : LocalizedMessage;
        
        public record EnteredDateTimeIsNotValid : LocalizedMessage;
        
        public record GroupIsNotSelected : LocalizedMessage;
        
        public record LecturerIsNotSelected : LocalizedMessage;
        
        public record DisciplineIsNotSelected : LocalizedMessage;
        
        public record ClassroomIsNotSelected : LocalizedMessage;

        public record DisciplineReferencedByLecturer(string LecturerName) : LocalizedMessage;
        
        public record DisciplineReferencedByExam(string GroupName) : LocalizedMessage;
        
        public record GroupReferencedByExam(string DisciplineName) : LocalizedMessage;
        
        public record ClassroomReferencedByExam(string GroupName, string DisciplineName) : LocalizedMessage;
        
        public record LecturerDoesNotAcceptDiscipline(string LecturerName, string DisciplineName) : LocalizedMessage;

        public record GroupDoesNotStudyDiscipline
            (string GroupName, string DisciplineName) : LocalizedMessage;
        
        public record LecturerReferencedByExam(string GroupName, string DisciplineName) : LocalizedMessage;

        public record SameExamAlreadyExists(string GroupName, string DisciplineName) : LocalizedMessage;
        
        public record DisciplineReferencedByGroup(string GroupName) : LocalizedMessage;

        public record NumberOfClassroomMustBeOriginal : LocalizedMessage;

        public record NameOfDisciplineMustBeOriginal : LocalizedMessage;
        
        public record NameOfGroupMustBeOriginal : LocalizedMessage;
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
        
        public record DeleteDiscipline : LocalizedMessage;
        
        public record DeleteLecturer : LocalizedMessage;
        
        public record DeleteGroup : LocalizedMessage;
        
        public record CloseLecturerEditor : LocalizedMessage;
        
        public record CloseGroupEditor : LocalizedMessage;
        
        public record CloseExamEditor : LocalizedMessage;

        public record DeleteExam : LocalizedMessage;
    }
}