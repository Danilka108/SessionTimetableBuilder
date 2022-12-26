using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageLecturer
(
    string Name,
    string Surname,
    string Patronymic,
    IEnumerable<LinkedEntity<StorageDiscipline>> Disciplines
);