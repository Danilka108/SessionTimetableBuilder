using System.Reactive.Linq;
using Adapters.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;
using Storage.Entity;
using Storage.StorageSet;

namespace Adapters.Project.Gateways;

public class ClassroomStorageGateway : IClassroomGateway
{
    private readonly Storage.Storage _storage;

    private readonly ClassroomFeatureStorageGateway _featureGateway;

    public ClassroomStorageGateway(Storage.Storage storage,
        ClassroomFeatureStorageGateway featureGateway)
    {
        _storage = storage;
        _featureGateway = featureGateway;
    }

    public async Task<Classroom> Create(int number, int capacity,
        IEnumerable<ClassroomFeature> features, CancellationToken token)
    {
        var featuresArray = features.ToArray();

        var linkedFeatures =
            featuresArray.Select(feature => new LinkedEntity<StorageClassroomFeature>(feature.Id));
        var storageClassroom = new StorageClassroom(number, capacity, linkedFeatures);

        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageClassroom>()
                .Add(storageClassroom, out var id)
                .Save();

            await t.Commit();

            return new Classroom(id, number, capacity, featuresArray);
        }
        catch (Exception e)
        {
            throw new ClassroomGatewayException("Failed to create classroom", e);
        }
    }

    public async Task Update(Classroom classroom, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageClassroom>()
                .Update(new IdentifiedEntity<StorageClassroom>(classroom.Id,
                    classroom.MapToStorageEntity()))
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new ClassroomGatewayException("Failed to update classroom", e);
        }
    }

    public async Task Delete(Classroom classroom, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageClassroom>()
                .Delete(classroom.Id)
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new ClassroomGatewayException("Failed to delete classroom", e);
        }
    }

    public async Task<Classroom> Read(int id, CancellationToken token)
    {
        foreach (var feature in await ReadAll(token))
        {
            if (feature.Id != id) continue;

            return feature;
        }

        throw new ClassroomGatewayException("Could not be found classroom");
    }

    public async Task<IEnumerable<Classroom>> ReadAll(CancellationToken token)
    {
        IEnumerable<IdentifiedEntity<StorageClassroom>> storageClassrooms;

        try
        {
            storageClassrooms = await _storage.FromSetOf<StorageClassroom>(token);
        }
        catch (Exception e)
        {
            throw new ClassroomFeatureGatewayException("Failed to read classrooms features", e);
        }

        var classroomsTasks =
            storageClassrooms.Select(storageClassroom =>
                _featureGateway.Read(storageClassroom.Entity.Features, token).ContinueWith(
                    async featuresTask =>
                    {
                        var features = await featuresTask;
                        return new Classroom(
                            storageClassroom.Id, storageClassroom.Entity.Number,
                            storageClassroom.Entity.Capacity, features
                        );
                    }, token));

        return await Task.WhenAll(await Task.WhenAll(classroomsTasks));
    }

    public IObservable<Classroom> Observe(int id)
    {
        return ObserveAll()
            .Select(classrooms =>
            {
                foreach (var classroom in classrooms)
                {
                    if (classroom.Id != id) continue;
                    return classroom;
                }

                throw new ClassroomGatewayException(
                    "Could not to be find classroom");
            });
    }

    public IObservable<IEnumerable<Classroom>> ObserveAll()
    {
        return _storage.ObserveFromSetOf<StorageClassroom>()
            .SelectMany(async (storageClassrooms, token) =>
            {
                var classrooms = new List<Classroom>();

                foreach (var storageClassroom in storageClassrooms)
                {
                    var features =
                        await _featureGateway.Read(storageClassroom.Entity.Features, token);

                    var classroom = new Classroom(storageClassroom.Id,
                        storageClassroom.Entity.Number, storageClassroom.Entity.Capacity, features);

                    classrooms.Add(classroom);
                }

                return classrooms;
            })
            .Catch<IEnumerable<Classroom>, Exception>(e =>
                throw new ClassroomGatewayException("Failed to observe classrooms", e)
            );
    }
}