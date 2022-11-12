using Data;

namespace ProjectData.Entities;

internal record BellTime(int Minute, int Hour)
{
    public class Helper : EntityModelHelper<BellTime, ProjectDomain.Models.BellTime>
    {
        public override BellTime ConvertModelToEntity(ProjectDomain.Models.BellTime model)
        {
            return new BellTime(model.Minute, model.Hour);
        }
    }
}