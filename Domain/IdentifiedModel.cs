namespace Domain;

public record IdentifiedModel<TModel>(int Id, TModel Model)
{
    public class EqualityComparer : EqualityComparer<IdentifiedModel<TModel>>
    {
        public override bool Equals(IdentifiedModel<TModel>? modelX, IdentifiedModel<TModel>? modelY)
        {
            if (modelX is null && modelY is null) return true;
            return modelX is not null && modelY is not null && modelX.Id == modelY.Id;
        }

        public override int GetHashCode(IdentifiedModel<TModel> model)
        {
            return model.Id;
        }
    }
}