namespace Domain.Models;

public record IdentifiedModel<TModel>(int Id, TModel Model);