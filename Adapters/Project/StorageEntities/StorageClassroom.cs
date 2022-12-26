using Storage.Entity;

namespace Adapters.Project.StorageEntities;

internal record StorageClassroom
(
    int Number,
    int Capacity,
    IEnumerable<LinkedEntity<StorageClassroomFeature>> Features
);