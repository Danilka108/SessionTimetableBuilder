namespace Data.Project.Entities;

internal record BellTime(int Minute, int Hour)
{
    public class Helper : EntityModelHelper<BellTime, Domain.Models.BellTime>
    {
        public override BellTime ConvertModelToEntity(Domain.Models.BellTime model)
        {
            return new BellTime(model.Minute, model.Hour);
        }
    }
}