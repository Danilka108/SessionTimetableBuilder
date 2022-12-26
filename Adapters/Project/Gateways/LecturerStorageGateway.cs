using System.Reactive.Linq;
using Adapters.Project.StorageEntities;
using Application.Project.Gateways;
using Domain.Project;
using Storage.Entity;
using Storage.StorageSet;

namespace Adapters.Project.Gateways;

public class LecturerStorageGateway : ILecturerGateway
{
    private readonly Storage.Storage _storage;

    private readonly DisciplineStorageGateway _disciplineGateway;

    public LecturerStorageGateway(Storage.Storage storage,
        DisciplineStorageGateway disciplineGateway)
    {
        _storage = storage;
        _disciplineGateway = disciplineGateway;
    }

    public async Task<Lecturer> Create(string name, string surname, string patronymic,
        IEnumerable<Discipline> disciplines,
        CancellationToken token)
    {
        var disciplinesArray = disciplines.ToArray();
        var linkedDisciplines = disciplinesArray.Select(discipline =>
            new LinkedEntity<StorageDiscipline>(discipline.Id));

        var storageLecturer = new StorageLecturer(name, surname, patronymic, linkedDisciplines);

        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageLecturer>()
                .Add(storageLecturer, out var id)
                .Save();

            await t.Commit();

            return new Lecturer(id, name, surname, patronymic, disciplinesArray);
        }
        catch (Exception e)
        {
            throw new LecturerGatewayException("Failed to create lecturer", e);
        }
    }

    public async Task Update(Lecturer lecturer, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);

            t
                .InSetOf<StorageLecturer>()
                .Update(new IdentifiedEntity<StorageLecturer>(lecturer.Id,
                    lecturer.MapToStorageEntity()))
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new LecturerGatewayException("Failed to update discipline", e);
        }
    }

    public async Task Delete(Lecturer lecturer, CancellationToken token)
    {
        try
        {
            await using var t = await _storage.StartTransaction(token);
            t
                .InSetOf<StorageLecturer>()
                .Delete(lecturer.Id)
                .Save();

            await t.Commit();
        }
        catch (Exception e)
        {
            throw new LecturerGatewayException("Failed to delete lecturer", e);
        }
    }

    public async Task<Lecturer> Read(int id, CancellationToken token)
    {
        foreach (var lecturer in await ReadAll(token))
        {
            if (lecturer.Id != id) continue;

            return lecturer;
        }

        throw new LecturerGatewayException("Could not be found lecturer");
    }

    public async Task<IEnumerable<Lecturer>> ReadAll(CancellationToken token)
    {
        IEnumerable<IdentifiedEntity<StorageLecturer>> storageLecturers;

        try
        {
            storageLecturers = await _storage.FromSetOf<StorageLecturer>(token);
        }
        catch (Exception e)
        {
            throw new LecturerGatewayException("Failed to read lecturers", e);
        }

        var lecturersTasks =
            storageLecturers.Select(storageLecturer =>
                _disciplineGateway.Read(storageLecturer.Entity.Disciplines, token)
                    .ContinueWith(
                        async disciplinesTask =>
                        {
                            var disciplines = await disciplinesTask;
                            return new Lecturer(
                                storageLecturer.Id, storageLecturer.Entity.Name,
                                storageLecturer.Entity.Surname, storageLecturer.Entity.Patronymic,
                                disciplines);
                        }, token));

        return await Task.WhenAll(await Task.WhenAll(lecturersTasks));
    }

    public IObservable<Lecturer> Observe(int id)
    {
        return ObserveAll()
            .Select(lecturers =>
            {
                foreach (var lecturer in lecturers)
                {
                    if (lecturer.Id != id) continue;
                    return lecturer;
                }

                throw new LecturerGatewayException("Could not be found lecturer");
            });
    }

    public IObservable<IEnumerable<Lecturer>> ObserveAll()
    {
        return _storage.ObserveFromSetOf<StorageLecturer>()
            .SelectMany(async (storageLecturers, token) =>
            {
                var storageLecturersArray = storageLecturers.ToArray();
                var lecturers = new List<Lecturer>();

                foreach (var storageLecturer in storageLecturersArray)
                {
                    var disciplines =
                        await _disciplineGateway.Read(storageLecturer.Entity.Disciplines,
                            token);

                    var lecturer = new Lecturer(storageLecturer.Id,
                        storageLecturer.Entity.Name, storageLecturer.Entity.Surname,
                        storageLecturer.Entity.Patronymic, disciplines);

                    lecturers.Add(lecturer);
                }

                return lecturers;
            })
            .Catch<IEnumerable<Lecturer>, Exception>(e =>
                throw new LecturerGatewayException("Failed to observe lecturers", e)
            );
    }
}