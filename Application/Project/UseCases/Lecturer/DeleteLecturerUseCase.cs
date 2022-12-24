namespace Application.Project.useCases.Lecturer;

// public class DeleteLecturerUseCase
// {
//     private readonly ILecturerGateway _gateway;
//
//     public DeleteLecturerUseCase(ILecturerGateway gateway)
//     {
//         _gateway = gateway;
//     }
//
//     public async Task Handle(int id)
//     {
//         var token = CancellationToken.None;
//         try
//         {
//             await _gateway.Delete(id, token);
//         }
//         catch (Exception e)
//         {
//             throw new DeleteLecturerException("Failed to delete lecturer.", e);
//         }
//     }
// }
//
// public class DeleteLecturerException : Exception
// {
//     public DeleteLecturerException(string msg, Exception innerException) : base(msg, innerException)
//     {
//     }
// }