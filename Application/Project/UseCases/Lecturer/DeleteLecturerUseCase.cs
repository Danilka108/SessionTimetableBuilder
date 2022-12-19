using Domain.Project.Repositories;

namespace Domain.Project.useCases.Teacher;

public class DeleteLecturerUseCase
{
    private readonly ITeacherGateWay _teacherGateWay;

    public DeleteLecturerUseCase(ITeacherGateWay teacherGateWay)
    {
        _teacherGateWay = teacherGateWay;
    }

    public async Task Handle(int id)
    {
        var token = CancellationToken.None;
        try
        {
            await _teacherGateWay.Delete(id, token);
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