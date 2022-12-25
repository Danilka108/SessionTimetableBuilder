using Application.Project.Gateways;
using Domain.Project;

namespace Application.Project.useCases.Lecturer;

public class SaveLecturerUseCase
{
    private readonly ILecturerGateway _gateway;

    public SaveLecturerUseCase(ILecturerGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(int? id, string name, string surname, string patronymic,
        IEnumerable<Discipline> disciplines, CancellationToken token)
    {
        if (id is { } notNullId)
        {
            var lecturer =
                new Domain.Project.Lecturer(notNullId, name, surname, patronymic, disciplines);
            await _gateway.Update(lecturer, token);
            return;
        }

        await _gateway.Create(name, surname, patronymic, disciplines, token);
    }
}