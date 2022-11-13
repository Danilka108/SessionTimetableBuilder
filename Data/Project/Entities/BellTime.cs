namespace Data.Project.Entities;

internal record BellTime(int Minute, int Hour)
{
    public class Helper : EntityModelHelper<BellTime, Domain.Project.Models.BellTime>
    {
        public override BellTime ConvertModelToEntity(Domain.Project.Models.BellTime model)
        {
            return new BellTime(model.Minute, model.Hour);
        }
    }
}