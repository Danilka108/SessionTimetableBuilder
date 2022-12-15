using Domain.Project.Repositories;

namespace Domain.Project.UseCases.BellTime;

public class SaveBellTimeUseCase
{
    private readonly IBellTimeRepository _repository;

    public SaveBellTimeUseCase(IBellTimeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(Models.BellTime bellTime, int? id = null)
    {
        ValidateBellTime(bellTime);

        var token = CancellationToken.None;

        await CheckToOriginality(bellTime, token, id);

        if (id is { } notNullId)
            await Update(notNullId, bellTime, token);
        else
            await Create(bellTime, token);
    }

    private void ValidateBellTime(Models.BellTime bellTime)
    {
        switch (bellTime.Hour)
        {
            case >= 24:
                throw new SaveBellTimeException("Hour value must less than 24.");
            case < 0:
                throw new SaveBellTimeException("Hour value must greater than or equal 0.");
        }

        switch (bellTime.Minute)
        {
            case >= 60:
                throw new SaveBellTimeException("Minute value must less than 60.");
            case < 0:
                throw new SaveBellTimeException("Hour value must greater than 0.");
        }
    }

    private async Task CheckToOriginality
        (Models.BellTime model, CancellationToken token, int? id = null)
    {
        var allBellTimes = await _repository.ReadAll(token);

        var sameBellTime = allBellTimes.FirstOrDefault
        (
            bellTime =>
                bellTime.Model.Minute == model.Minute && bellTime.Model.Hour == model.Hour
        );

        if (sameBellTime?.Id == id) return;

        if (sameBellTime is { })
            throw new SaveBellTimeException
                ("Bell time must be original.");
    }

    private async Task Create(Models.BellTime model, CancellationToken token)
    {
        try
        {
            await _repository.Create(model, token);
        }
        catch (Exception e)
        {
            throw new SaveBellTimeException("Failed to create bell time.", e);
        }
    }

    private async Task Update(int id, Models.BellTime model, CancellationToken token)
    {
        try
        {
            await _repository.Update(new IdentifiedModel<Models.BellTime>(id, model), token);
        }
        catch (Exception e)
        {
            throw new SaveBellTimeException("Failed to update bell time", e);
        }
    }
}

public class SaveBellTimeException : Exception
{
    internal SaveBellTimeException(string msg) : base(msg)
    {
    }

    internal SaveBellTimeException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}