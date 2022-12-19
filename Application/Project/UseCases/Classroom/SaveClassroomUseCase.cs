using Application.Project.Gateways;
using Domain.Project;

namespace Application.Project.UseCases.Classroom;

public class SaveClassroomUseCase
{
    private readonly IClassroomGateway _gateway;

    public SaveClassroomUseCase(IClassroomGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(Domain.Project.Classroom classroom, int? id = null)
    {
        var token = CancellationToken.None;

        await CheckNumberToOriginality(token, classroom.Number, id);

        if (id is { } notNullId)
        {
            await Update(classroom, notNullId, token);
            return;
        }

        await Create(classroom, token);
    }

    private async Task CheckNumberToOriginality(CancellationToken token, int number, int? id = null)
    {
        var allClassroom = await _gateway.ReadAll(token);
        var classroomWithSameNumber = allClassroom.FirstOrDefault(a => a.Model.Number == number);

        if (classroomWithSameNumber?.Id == id) return;

        if (classroomWithSameNumber is { })
            throw new SaveClassroomException("Number of classroom must be original");
    }

    private async Task Create(Domain.Project.Classroom classroom, CancellationToken token)
    {
        try
        {
            await _gateway.Create(classroom, token);
        }
        catch (Exception e)
        {
            throw new SaveClassroomException
                ($"Failed to create classroom with number '{classroom.Number}'", e);
        }
    }

    private async Task Update(Domain.Project.Classroom classroom, int id, CancellationToken token)
    {
        try
        {
            await _gateway.Update(new Identified<Domain.Project.Classroom>(id, classroom),
                token);
        }
        catch (Exception e)
        {
            throw new SaveClassroomException
                ($"Failed to update classroom with number '{classroom.Number}'", e);
        }
    }
}

public class SaveClassroomException : Exception
{
    internal SaveClassroomException(string msg) : base(msg)
    {
    }

    internal SaveClassroomException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}