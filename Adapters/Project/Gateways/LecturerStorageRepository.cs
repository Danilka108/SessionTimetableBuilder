using Adapters.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;

namespace Adapters.Project.Gateways;

internal class LecturerStorageRepository : BaseStorageGateway<Lecturer, StorageLecturer>,
    ILecturerGateway
{
    private readonly IDisciplineGateway _disciplineGateway;

    public LecturerStorageRepository(Storage.Storage storage, IDisciplineGateway disciplineGateway)
        : base(storage, new StorageLecturer.Converter())
    {
        _disciplineGateway = disciplineGateway;
    }

    protected override async Task<Lecturer> ProduceEntity(StorageLecturer storageEntity,
        CancellationToken token)
    {
        var disciplines = new List<Identified<Discipline>>();

        foreach (var linkedDiscipline in storageEntity.Disciplines)
        {
            var discipline = await _disciplineGateway.Read(linkedDiscipline.Id, token);
            disciplines.Add(discipline);
        }

        return new Lecturer(storageEntity.Name, storageEntity.Surname, storageEntity.Patronymic,
            disciplines);
    }
}