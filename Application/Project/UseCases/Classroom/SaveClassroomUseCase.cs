using Application.Project.Gateways;

namespace Application.Project.UseCases.Classroom;

public class SaveClassroomUseCase
{
    private readonly IClassroomGateway _gateway;

    public SaveClassroomUseCase(IClassroomGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(int number, int capacity,
        IEnumerable<Domain.Project.ClassroomFeature> features, int? id = null)
    {
        var token = CancellationToken.None;

        await CheckNumberToOriginality(token, number, id);

        if (id is { } notNullId)
        {
            var classroom = new Domain.Project.Classroom(notNullId, number, capacity, features);
            await _gateway.Update(classroom, token);
            return;
        }

        await _gateway.Create(number, capacity, features, token);
    }

    private async Task CheckNumberToOriginality(CancellationToken token, int number, int? id = null)
    {
        var allClassrooms = await _gateway.ReadAll(token);
        var classroomWithSameNumber =
            allClassrooms.FirstOrDefault(classroom => classroom.Number == number);

        if (classroomWithSameNumber?.Id == id) return;

        if (classroomWithSameNumber is { })
            throw new ClassroomNumberMustBeOriginalException();
    }
}

public class ClassroomNumberMustBeOriginalException : Exception
{
}