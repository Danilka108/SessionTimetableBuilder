using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

public class DeleteDisciplineUseCase
{
    private readonly IDisciplineGateway _gateway;

    private readonly ILecturerGateway _lecturerGateway;

    public DeleteDisciplineUseCase(IDisciplineGateway gateway, ILecturerGateway lecturerGateway)
    {
        _gateway = gateway;
        _lecturerGateway = lecturerGateway;
    }

    public async Task Handle(Domain.Project.Discipline discipline, CancellationToken token)
    {
        var allLecturers = await _lecturerGateway.ReadAll(token);

        foreach (var lecturer in allLecturers)
        {
            var contains = lecturer.ContainsDiscipline(discipline);

            if (contains)
            {
                throw new DisciplineReferencedByLecturerException(lecturer);
            }
        }
        
        await _gateway.Delete(discipline, token);
    }
}

public class DisciplineReferencedByLecturerException : Exception
{
    public DisciplineReferencedByLecturerException(
        Domain.Project.Lecturer lecturer)
    {
        Lecturer = lecturer;
    }

    public Domain.Project.Lecturer Lecturer { get; }
}
