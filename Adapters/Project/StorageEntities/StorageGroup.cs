using Domain.Project;
using Storage;
using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageGroup
(
    string Name,
    int StudentsNumber,
    IEnumerable<LinkedEntity<StorageDiscipline>> Disciplines
);