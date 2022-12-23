using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

// public class DeleteDisciplineUseCase
// {
//     private readonly IDisciplineGateway _gateway;
//
//     public DeleteDisciplineUseCase(IDisciplineGateway gateway)
//     {
//         _gateway = gateway;
//     }
//
//     public async Task Handle(int id)
//     {
//         var token = CancellationToken.None;
//
//         try
//         {
//             await _gateway.Delete(id, token);
//         }
//         catch (Exception e)
//         {
//             throw new DeleteDisciplineException("Failed to delete discipline.", e);
//         }
//     }
// }
//
// public class DeleteDisciplineException : Exception
// {
//     internal DeleteDisciplineException(string msg, Exception innerException) : base
//         (msg, innerException)
//     {
//     }
// }