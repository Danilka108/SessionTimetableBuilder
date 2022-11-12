using System.Reactive.Linq;
using Data.ProjectStorage;
using Domain.Repositories;
using AudienceSpecificity = Domain.Models.AudienceSpecificity;

namespace Data.Repositories;

public class AudienceSpecificityRepository : IAudienceSpecificityRepository
{
    private readonly Storage.Storage _storage;

    public AudienceSpecificityRepository(ProjectStorageProvider storageProvider)
    {
        _storage = storageProvider.ProvideStorage();
    }

    public async Task<AudienceSpecificity> Create(string description)
    {
        var token = CancellationToken.None;

        var entity = new ProjectStorage.AudienceSpecificity(description);

        await using var transaction = await _storage.StartTransaction(token);
        transaction
            .InSetOf<ProjectStorage.AudienceSpecificity>()
            .Add(entity, out var id)
            .Save();
        await transaction.Commit();

        return entity.MapToDomainModel(id);
    }

    public async Task Update(AudienceSpecificity audienceSpecificity)
    {
        var token = CancellationToken.None;

        await using var transaction = await _storage.StartTransaction(token);
        transaction
            .InSetOf<ProjectStorage.AudienceSpecificity>()
            .Update(ProjectStorage.AudienceSpecificity
                .FromDomainModel(audienceSpecificity)
            )
            .Save();
        await transaction.Commit();
    }

    public async Task Delete(AudienceSpecificity audienceSpecificity)
    {
        var token = CancellationToken.None;

        await using var transaction = await _storage.StartTransaction(token);
        transaction
            .InSetOf<ProjectStorage.AudienceSpecificity>()
            .Delete(audienceSpecificity.Id)
            .Save();
        await transaction.Commit();
    }

    public IObservable<AudienceSpecificity> Observe(int id)
    {
        return _storage
            .ObserveFromSetOf<ProjectStorage.AudienceSpecificity>()
            .Select
            (entities =>
                entities.ToList().Find(e => e.Id == id)
            )
            .Where(e => e is { })
            .Select(e => e!.Entity.MapToDomainModel(e.Id));
    }
}