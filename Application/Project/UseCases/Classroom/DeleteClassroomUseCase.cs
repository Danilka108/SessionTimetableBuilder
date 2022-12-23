
using Application.Project.Gateways;

namespace Application.Project.UseCases.Classroom;

// public class DeleteClassroomUseCase
// {
//     private readonly IClassroomGateway _gateway;
//
//     public DeleteClassroomUseCase(IClassroomGateway gateway)
//     {
//         _gateway = gateway;
//     }
//
//     public async Task Handle(int id)
//     {
//         try
//         {
//             var token = CancellationToken.None;
//             await _gateway.Delete(id, token);
//         }
//         catch (Exception e)
//         {
//             throw new DeleteClassroomException("Failed to delete classroom", e);
//         }
//     }
// }
//
// public class DeleteClassroomException : Exception
// {
//     internal DeleteClassroomException(string msg, Exception innerException) : base(msg,
//         innerException)
//     {
//     }
// }