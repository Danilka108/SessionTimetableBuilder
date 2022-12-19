using Application.Project.Gateways;

namespace Application.Project.useCases.Lecturer;

public class SaveLecturerUseCase
{
    private readonly ILecturerGateway _gateway;

    public SaveLecturerUseCase(ITeacherGateWay gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(Models.Teacher model, int? id = null)
    {
        var token = CancellationToken.None;

        await CheckToOriginality(token, model, id);

        if (id is { } notNullId)
            await Update(model, notNullId, token);
        else
            await Create(model, token);
    }

    private async Task CheckToOriginality
        (CancellationToken token, Models.Teacher model, int? id = null)
    {
        var allDisciplines = await _gateway.ReadAll(token);

        var disciplineWithSameName =
            allDisciplines.FirstOrDefault
            (
                d => d.Model.Name == model.Name && d.Model.Surname == model.Surname &&
                     d.Model.Patronymic == model.Patronymic
            );

        if (disciplineWithSameName?.Id == id) return;

        if (disciplineWithSameName is { })
            throw new SaveTeacherException("Full name of teacher must be original");
    }

    private async Task Create(Models.Teacher model, CancellationToken token)
    {
        try
        {
            await _gateway.Create(model, token);
        }
        catch (Exception e)
        {
            throw new SaveTeacherException("Failed to create teacher.", e);
        }
    }

    private async Task Update(Models.Teacher model, int id, CancellationToken token)
    {
        try
        {
            await _gateway.Update(new IdentifiedModel<Models.Teacher>(id, model), token);
        }
        catch (Exception e)
        {
            throw new SaveTeacherException("Failed to update teacher.", e);
        }
    }
}

public class SaveTeacherException : Exception
{
    internal SaveTeacherException(string msg) : base(msg)
    {
    }

    internal SaveTeacherException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}