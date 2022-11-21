using Domain.Project.Repositories;

namespace Domain.Project.UseCases.Discipline;

public class DeleteDisciplineUseCase
{
    private readonly IDisciplineRepository _repository;

    public DeleteDisciplineUseCase(IDisciplineRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(int id)
    {
        var token = CancellationToken.None;

        try
        {
            await _repository.Delete(id, token);
        }
        catch (Exception e)
        {
            throw new DeleteDisciplineException("Failed to delete discipline.", e);
        }
    }
}

public class DeleteDisciplineException : Exception
{
    internal DeleteDisciplineException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}