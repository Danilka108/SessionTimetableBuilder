using Domain.Project.Repositories;

namespace Domain.Project.UseCases.BellTime;

public class DeleteBellTimeUseCase
{
    private readonly IBellTimeRepository _repository;

    public DeleteBellTimeUseCase(IBellTimeRepository repository)
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
            throw new DeleteBellTimeException("Failed to delete bell time", e);
        }
    }
}

public class DeleteBellTimeException : Exception
{
    internal DeleteBellTimeException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}