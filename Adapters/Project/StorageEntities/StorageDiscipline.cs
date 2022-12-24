using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageDiscipline
(
    string Name,
    IEnumerable<LinkedEntity<StorageClassroomFeature>> ClassroomRequirements
);