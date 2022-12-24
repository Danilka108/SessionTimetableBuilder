using System.Reactive.Linq;
using Adapters.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;
using Storage.Entity;
using Storage.StorageSet;

namespace Adapters.Project.Gateways;

public class DisciplineStorageGateway : IDisciplineGateway
{
    private readonly ClassroomFeatureStorageGateway _featureGateway;
    private readonly Storage.Storage _storage;

    public DisciplineStorageGateway(Storage.Storage storage,
        ClassroomFeatureStorageGateway featureGateway)
    {
        _storage = storage;
        _featureGateway = featureGateway;
    }

    public async Task<Discipline> Create(string name,
        IEnumerable<ClassroomFeature> classroomRequirements,
        CancellationToken token)
    {
        var requirementsArray = classroomRequirements.ToArray();

        var linkedRequirements =
            requirementsArray.Select(requirement =>
                new LinkedEntity<StorageClassroomFeature>(requirement.Id));

        var storageDiscipline = new StorageDiscipline(name, linkedRequirements);

        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageDiscipline>()
                .Add(storageDiscipline, out var id)
                .Save();

            await t.Commit();

            return new Discipline(id, name, requirementsArray);
        }
        catch (Exception e)
        {
            throw new DisciplineGatewayException("Failed to create discipline", e);
        }
    }

    public async Task Update(Discipline discipline, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);

            t
                .InSetOf<StorageDiscipline>()
                .Update(new IdentifiedEntity<StorageDiscipline>(discipline.Id,
                    discipline.MapToStorageEntity()))
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new DisciplineGatewayException("Failed to update discipline", e);
        }
    }

    public async Task Delete(Discipline discipline, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageDiscipline>()
                .Delete(discipline.Id)
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new DisciplineGatewayException("Failed to delete discipline", e);
        }
    }

    public async Task<Discipline> Read(int id, CancellationToken token)
    {
        foreach (var discipline in await ReadAll(token))
        {
            if (discipline.Id != id) continue;

            return discipline;
        }

        throw new DisciplineGatewayException("Could not be found discipline");
    }

    public async Task<IEnumerable<Discipline>> ReadAll(CancellationToken token)
    {
        IEnumerable<IdentifiedEntity<StorageDiscipline>> storageDisciplines;

        try
        {
            storageDisciplines = await _storage.FromSetOf<StorageDiscipline>(token);
        }
        catch (Exception e)
        {
            throw new DisciplineGatewayException("Failed to read disciplines", e);
        }

        var classroomsTasks =
            storageDisciplines.Select(storageDiscipline =>
                _featureGateway.Read(storageDiscipline.Entity.ClassroomRequirements, token)
                    .ContinueWith(
                        async requirementsTask =>
                        {
                            var requirements = await requirementsTask;
                            return new Discipline(
                                storageDiscipline.Id, storageDiscipline.Entity.Name, requirements);
                        }, token));

        return await Task.WhenAll(await Task.WhenAll(classroomsTasks));
    }

    public IObservable<Discipline> Observe(int id)
    {
        return ObserveAll()
            .Select(disciplines =>
            {
                foreach (var discipline in disciplines)
                {
                    if (discipline.Id != id) continue;
                    return discipline;
                }

                throw new DisciplineGatewayException("Could not be found discipline");
            });
    }

    public IObservable<IEnumerable<Discipline>> ObserveAll()
    {
        return _storage.ObserveFromSetOf<StorageDiscipline>()
            .SelectMany(async (storageDisciplines, token) =>
            {
                var storageDisciplinesArray = storageDisciplines.ToArray();
                var disciplines = new List<Discipline>();

                foreach (var storageDiscipline in storageDisciplinesArray)
                {
                    var requirements =
                        await _featureGateway.Read(storageDiscipline.Entity.ClassroomRequirements,
                            token);

                    var discipline = new Discipline(storageDiscipline.Id,
                        storageDiscipline.Entity.Name, requirements);

                    disciplines.Add(discipline);
                }

                return disciplines;
            })
            .Catch<IEnumerable<Discipline>, Exception>(e =>
                throw new DisciplineGatewayException("Failed to observe disciplines", e)
            );
    }
}