using Data.ProjectStorage;
using Domain.Models;
using Domain.Repositories;
using Storage;

namespace Data.Repositories;

public class AudienceRepository : IAudienceRepository
{
    private readonly Storage.Storage _storage;

    public AudienceRepository(ProjectStorageProvider storageProvider)
    {
        _storage = storageProvider.ProvideStorage();
    }

    public async Task<Audience> Create(int number, int capacity, IEnumerable<AudienceSpecificity> specificities)
    {
        var entity = CreateEntity(number, capacity, specificities);

        var token = CancellationToken.None;

        await using var transaction = await _storage.StartTransaction(token);

        transaction
            .InSetOf<ProjectStorage.Audience>()
            .Add(entity, out var id)
            .Save();

        await transaction.Commit();


        return new Audience(id, entity.)
    }

    public Task Update(Audience audience)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IObservable<Audience> Observe(int id)
    {
        throw new NotImplementedException();
    }


    private ProjectStorage.Audience CreateEntity(int number, int capacity,
        IEnumerable<AudienceSpecificity> specificities)
    {
        var linkedSpecificities = specificities
            .Select(s =>
                new LinkedEntity<ProjectStorage.AudienceSpecificity>(s.Id)
            );

        return new ProjectStorage.Audience(number, capacity, linkedSpecificities);
    }
}