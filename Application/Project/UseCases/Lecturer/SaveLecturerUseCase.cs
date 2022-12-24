namespace Application.Project.useCases.Lecturer;

// public class SaveLecturerUseCase
// {
//     private readonly ILecturerGateway _gateway;
//
//     public SaveLecturerUseCase(ILecturerGateway gateway)
//     {
//         _gateway = gateway;
//     }
//
//     public async Task Handle(Domain.Project.Lecturer model, int? id = null)
//     {
//         var token = CancellationToken.None;
//
//         await CheckToOriginality(token, model, id);
//
//         if (id is { } notNullId)
//             await Update(model, notNullId, token);
//         else
//             await Create(model, token);
//     }
//
//     private async Task CheckToOriginality
//         (CancellationToken token, Domain.Project.Lecturer lecturer, int? id = null)
//     {
//         var allLecturers = await _gateway.ReadAll(token);
//
//         var lecturerWithSameName =
//             allLecturers.FirstOrDefault
//             (
//                 d => d.Entity.Name == lecturer.Name && d.Entity.Surname == lecturer.Surname &&
//                      d.Entity.Patronymic == lecturer.Patronymic
//             );
//
//         if (lecturerWithSameName?.Id == id) return;
//
//         if (lecturerWithSameName is { })
//             throw new SaveLecturerException("Full name of lecturer must be original");
//     }
//
//     private async Task Create(Domain.Project.Lecturer lecturer, CancellationToken token)
//     {
//         try
//         {
//             await _gateway.Create(lecturer, token);
//         }
//         catch (Exception e)
//         {
//             throw new SaveLecturerException("Failed to create lecturer.", e);
//         }
//     }
//
//     private async Task Update(Domain.Project.Lecturer lecturer, int id, CancellationToken token)
//     {
//         try
//         {
//             await _gateway.Update(new Identified<Domain.Project.Lecturer>(id, lecturer), token);
//         }
//         catch (Exception e)
//         {
//             throw new SaveLecturerException("Failed to update lecturer.", e);
//         }
//     }
// }
//
// public class SaveLecturerException : Exception
// {
//     internal SaveLecturerException(string msg) : base(msg)
//     {
//     }
//
//     internal SaveLecturerException(string msg, Exception innerException) : base
//         (msg, innerException)
//     {
//     }
// }