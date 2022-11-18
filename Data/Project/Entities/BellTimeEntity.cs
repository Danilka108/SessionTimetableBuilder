using Domain.Project.Models;

namespace Data.Project.Entities;

internal record BellTimeEntity(int Minute, int Hour)
{
    public class Helper : EntityModelHelper<BellTimeEntity, BellTime>
    {
        public override BellTimeEntity ConvertModelToEntity(BellTime model)
        {
            return new BellTimeEntity(model.Minute, model.Hour);
        }
    }
}