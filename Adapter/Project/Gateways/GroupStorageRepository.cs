using Adapter.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;

namespace Adapter.Project.Gateways;

internal class GroupStorageRepository : BaseStorageGateway<Group, StorageGroup>, IGroupGateway
{
    private readonly IDisciplineGateway _disciplineGateway;

    public GroupStorageRepository(Storage.Storage storage, IDisciplineGateway disciplineGateway)
        : base(storage, new StorageGroup.Converter())
    {
        _disciplineGateway = disciplineGateway;
    }

    protected override async Task<Group> ProduceEntity(StorageGroup storageEntity,
        CancellationToken token)
    {
        var disciplines = new List<Identified<Discipline>>();

        foreach (var linkedDiscipline in storageEntity.Disciplines)
        {
            var discipline = await _disciplineGateway.Read(linkedDiscipline.Id, token);
            disciplines.Add(discipline);
        }

        return new Group(storageEntity.Name, storageEntity.StudentsNumber, disciplines);
    }
}