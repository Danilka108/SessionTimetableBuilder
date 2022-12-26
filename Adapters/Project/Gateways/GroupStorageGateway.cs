using System.Reactive.Linq;
using Adapters.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;
using Storage.Entity;
using Storage.StorageSet;

namespace Adapters.Project.Gateways;

public class GroupStorageGateway : IGroupGateway
{
    private readonly Storage.Storage _storage;

    private readonly DisciplineStorageGateway _disciplineGateway;

    public GroupStorageGateway(Storage.Storage storage, DisciplineStorageGateway disciplineGateway)
    {
        _storage = storage;
        _disciplineGateway = disciplineGateway;
    }

    public async Task<Group> Create(string name, int studentsNumber,
        IEnumerable<Discipline> disciplines, CancellationToken token)
    {
        var disciplinesArray = disciplines.ToArray();
        var linkedDisciplines = disciplinesArray.Select(discipline =>
            new LinkedEntity<StorageDiscipline>(discipline.Id));

        var storageGroup = new StorageGroup(name, studentsNumber, linkedDisciplines);

        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageGroup>()
                .Add(storageGroup, out var id)
                .Save();

            await t.Commit();

            return new Group(id, name, studentsNumber, disciplinesArray);
        }
        catch (Exception e)
        {
            throw new GroupGatewayException("Failed to create group", e);
        }
    }

    public async Task Update(Group group, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);

            t
                .InSetOf<StorageGroup>()
                .Update(new IdentifiedEntity<StorageGroup>(group.Id,
                    group.MapToStorageEntity()))
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new GroupGatewayException("Failed to update group", e);
        }
    }

    public async Task Delete(Group group, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageGroup>()
                .Delete(group.Id)
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new GroupGatewayException("Failed to delete group", e);
        }
    }

    public async Task<Group> Read(int id, CancellationToken token)
    {
        foreach (var group in await ReadAll(token))
        {
            if (group.Id != id) continue;

            return group;
        }

        throw new GroupGatewayException("Could not be found group");
    }

    public async Task<IEnumerable<Group>> ReadAll(CancellationToken token)
    {
        IEnumerable<IdentifiedEntity<StorageGroup>> storageGroups;

        try
        {
            storageGroups = await _storage.FromSetOf<StorageGroup>(token);
        }
        catch (Exception e)
        {
            throw new GroupGatewayException("Failed to read groups", e);
        }

        var groupsTasks =
            storageGroups.Select(storageGroup =>
                _disciplineGateway.Read(storageGroup.Entity.Disciplines, token)
                    .ContinueWith(
                        async disciplinesTask =>
                        {
                            var disciplines = await disciplinesTask;
                            return new Group(
                                storageGroup.Id, storageGroup.Entity.Name,
                                storageGroup.Entity.StudentsNumber, disciplines);
                        }, token));

        return await Task.WhenAll(await Task.WhenAll(groupsTasks));
    }

    public IObservable<Group> Observe(int id)
    {
        return ObserveAll()
            .Select(groups =>
            {
                foreach (var group in groups)
                {
                    if (group.Id != id) continue;
                    return group;
                }

                throw new GroupGatewayException("Could not be found group");
            });
    }

    public IObservable<IEnumerable<Group>> ObserveAll()
    {
        return _storage.ObserveFromSetOf<StorageGroup>()
            .SelectMany(async (storageGroups, token) =>
            {
                var storageGroupsArray = storageGroups.ToArray();
                var groups = new List<Group>();

                foreach (var storageGroup in storageGroupsArray)
                {
                    var disciplines =
                        await _disciplineGateway.Read(storageGroup.Entity.Disciplines,
                            token);

                    var group = new Group(storageGroup.Id,
                        storageGroup.Entity.Name, storageGroup.Entity.StudentsNumber, disciplines);

                    groups.Add(group);
                }

                return groups;
            })
            .Catch<IEnumerable<Group>, Exception>(e =>
                throw new GroupGatewayException("Failed to observe all groups", e)
            );
    }
}