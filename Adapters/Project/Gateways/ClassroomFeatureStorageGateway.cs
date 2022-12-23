using System.Reactive.Linq;
using Adapters.Project.StorageEntities;
using Application.Project;
using Application.Project.Gateways;
using Domain.Project;
using Storage.StorageSet;

namespace Adapters.Project.Gateways;

internal class ClassroomFeatureStorageGateway : IClassroomFeatureGateway
{
    private readonly Storage.Storage _storage;

    public ClassroomFeatureStorageGateway(Storage.Storage storage)
    {
        _storage = storage;
    }

    public async Task<ClassroomFeature> Create(string description, CancellationToken token)
    {
        var storageFeature = new StorageClassroomFeature(description);

        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageClassroomFeature>()
                .Add(storageFeature, out var id)
                .Save();

            await t.Commit();

            return new ClassroomFeature(id, description);
        }
        catch (Exception e)
        {
            throw new ClassroomFeatureGatewayException("Failed to create classroom feature", e);
        }
    }

    public async Task Update(ClassroomFeature feature, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageClassroomFeature>()
                .Update(new IdentifiedEntity<StorageClassroomFeature>(feature.Id,
                    feature.MapToStorageEntity()))
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new ClassroomFeatureGatewayException("Failed to update classroom feature", e);
        }
    }

    public async Task Delete(ClassroomFeature feature, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageClassroomFeature>()
                .Delete(feature.Id)
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new ClassroomFeatureGatewayException("Failed to delete classroom feature", e);
        }
    }

    public async Task<ClassroomFeature> Read(int id, CancellationToken token)
    {
        foreach (var feature in await ReadAll(token))
        {
            if (feature.Id != id) continue;

            return feature;
        }

        throw new ClassroomFeatureGatewayException("Could not be found classroom feature");
    }

    public async Task<IEnumerable<ClassroomFeature>> ReadAll(CancellationToken token)
    {
        IEnumerable<IdentifiedEntity<StorageClassroomFeature>> features;

        try
        {
            features = await _storage.FromSetOf<StorageClassroomFeature>(token);
        }
        catch (Exception e)
        {
            throw new ClassroomFeatureGatewayException("Failed to read classrooms features", e);
        }

        return features.Select(feature =>
        {
            return new ClassroomFeature(feature.Id, feature.Entity.Description);
        });
    }

    public IObservable<ClassroomFeature> Observe(int id)
    {
        return ObserveAll()
            .Select(features =>
            {
                foreach (var feature in features)
                {
                    if (feature.Id != id) continue;
                    return feature;
                }

                throw new ClassroomFeatureGatewayException(
                    "Could not to be find classroom feature");
            });
    }

    public IObservable<IEnumerable<ClassroomFeature>> ObserveAll()
    {
        return _storage.ObserveFromSetOf<StorageClassroomFeature>()
            .Select(features =>
            {
                var mappedFeatures = features.Select(feature =>
                    new ClassroomFeature(feature.Id, feature.Entity.Description));

                return mappedFeatures;
            })
            .Catch<IEnumerable<ClassroomFeature>, Exception>(e =>
                throw new ClassroomFeatureGatewayException("Failed to observe features", e)
            );
    }
}