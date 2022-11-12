namespace Domain.Models;

public interface IdentifiedModel<out TModel>
{
    int Id { get; }

    TModel Model { get; }
}