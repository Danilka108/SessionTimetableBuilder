using Data;
using Storage;

namespace ProjectData.Entities;

internal record Exam
(
    LinkedEntity<Teacher> Teacher,
    LinkedEntity<Group> Group,
    LinkedEntity<Discipline> Discipline,
    LinkedEntity<Audience> Audience,
    LinkedEntity<BellTime> StartBellTime,
    LinkedEntity<BellTime> EndBellTime,
    int Day,
    int Month,
    int Year
)
{
    public class Helper : EntityModelHelper<Exam, ProjectDomain.Models.Exam>
    {
        public override Exam ConvertModelToEntity(ProjectDomain.Models.Exam model)
        {
            return new Exam(
                new LinkedEntity<Teacher>(model.Teacher.Id),
                new LinkedEntity<Group>(model.Group.Id),
                new LinkedEntity<Discipline>(model.Discipline.Id),
                new LinkedEntity<Audience>(model.Audience.Id),
                new LinkedEntity<BellTime>(model.StartBellTime.Id),
                new LinkedEntity<BellTime>(model.EndBellTime.Id),
                model.Day, model.Month, model.Year
            );
        }
    }
}