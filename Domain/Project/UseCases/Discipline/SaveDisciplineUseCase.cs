using Domain.Project.Repositories;

namespace Domain.Project.UseCases.Discipline;

public class SaveDisciplineUseCase
{
    private readonly IDisciplineRepository _repository;

    public SaveDisciplineUseCase(IDisciplineRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(Models.Discipline model, int? id = null)
    {
        var token = CancellationToken.None;

        await CheckNameToOriginality(token, model, id);

        if (id is { } notNullId)
            await Update(model, notNullId, token);
        else
            await Create(model, token);
    }

    private async Task CheckNameToOriginality
        (CancellationToken token, Models.Discipline model, int? id = null)
    {
        var allDisciplines = await _repository.ReadAll(token);

        var disciplineWithSameName =
            allDisciplines.FirstOrDefault(d => d.Model.Name == model.Name);

        if (disciplineWithSameName?.Id == id) return;

        if (disciplineWithSameName is { })
            throw new SaveDisciplineException("Name of discipline must be original");
    }

    private async Task Create(Models.Discipline model, CancellationToken token)
    {
        try
        {
            await _repository.Create(model, token);
        }
        catch (Exception e)
        {
            throw new SaveDisciplineException("Failed to create discipline.", e);
        }
    }

    private async Task Update(Models.Discipline model, int id, CancellationToken token)
    {
        try
        {
            await _repository.Update(new IdentifiedModel<Models.Discipline>(id, model), token);
        }
        catch (Exception e)
        {
            throw new SaveDisciplineException("Failed to update discipline.", e);
        }
    }
}

public class SaveDisciplineException : Exception
{
    internal SaveDisciplineException(string msg) : base(msg)
    {
    }

    internal SaveDisciplineException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}