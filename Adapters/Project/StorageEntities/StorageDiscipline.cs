using Domain.Project;
using Storage;
using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageDiscipline
(
    string Name,
    IEnumerable<LinkedEntity<StorageClassroomFeature>> ClassroomRequirements
);