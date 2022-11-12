namespace Domain;

public record IdentifiedModel<TModel>(int Id, TModel Model);