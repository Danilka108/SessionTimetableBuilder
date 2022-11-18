using Domain.Project.Models;
using Storage;

namespace Data.Project.Entities;

internal record ExamEntity
(
    LinkedEntity<TeacherEntity> Teacher,
    LinkedEntity<GroupEntity> Group,
    LinkedEntity<DisciplineEntity> Discipline,
    LinkedEntity<AudienceEntity> Audience,
    LinkedEntity<BellTimeEntity> StartBellTime,
    LinkedEntity<BellTimeEntity> EndBellTime,
    int Day,
    int Month,
    int Year
)
{
    public class Helper : EntityModelHelper<ExamEntity, Exam>
    {
        public override ExamEntity ConvertModelToEntity(Exam model)
        {
            return new ExamEntity
            (
                new LinkedEntity<TeacherEntity>(model.Teacher.Id),
                new LinkedEntity<GroupEntity>(model.Group.Id),
                new LinkedEntity<DisciplineEntity>(model.Discipline.Id),
                new LinkedEntity<AudienceEntity>(model.Audience.Id),
                new LinkedEntity<BellTimeEntity>(model.StartBellTime.Id),
                new LinkedEntity<BellTimeEntity>(model.EndBellTime.Id),
                model.Day,
                model.Month,
                model.Year
            );
        }
    }
}