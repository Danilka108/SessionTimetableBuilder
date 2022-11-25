using Domain.Project.Repositories;

namespace Domain.Project.useCases.Teacher;

public class DeleteTeacherUseCase
{
    private readonly ITeacherRepository _teacherRepository;

    public DeleteTeacherUseCase(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task Handle(int id)
    {
        var token = CancellationToken.None;
        try
        {
            await _teacherRepository.Delete(id, token);
        }
        catch (Exception e)
        {
            throw new DeleteTeacherException("Failed to delete teacher.", e);
        }
    }
}

public class DeleteTeacherException : Exception
{
    public DeleteTeacherException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}