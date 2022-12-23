using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

// public class SaveDisciplineUseCase
// {
//     private readonly IDisciplineGateway _gateway;
//
//     public SaveDisciplineUseCase(IDisciplineGateway gateway)
//     {
//         _gateway = gateway;
//     }
//
//     public async Task Handle(Domain.Project.Discipline entity, int? id = null)
//     {
//         var token = CancellationToken.None;
//
//         await CheckNameToOriginality(token, entity, id);
//
//         if (id is { } notNullId)
//             await Update(entity, notNullId, token);
//         else
//             await Create(entity, token);
//     }
//
//     private async Task CheckNameToOriginality
//         (CancellationToken token, Domain.Project.Discipline entity, int? id = null)
//     {
//         var allDisciplines = await _gateway.ReadAll(token);
//
//         var disciplineWithSameName =
//             allDisciplines.FirstOrDefault(d => d.Entity.Name == entity.Name);
//
//         if (disciplineWithSameName?.Id == id) return;
//
//         if (disciplineWithSameName is { })
//             throw new SaveDisciplineException("Name of discipline must be original");
//     }
//
//     private async Task Create(Domain.Project.Discipline model, CancellationToken token)
//     {
//         try
//         {
//             await _gateway.Create(model, token);
//         }
//         catch (Exception e)
//         {
//             throw new SaveDisciplineException("Failed to create discipline.", e);
//         }
//     }
//
//     private async Task Update(Domain.Project.Discipline entity, int id, CancellationToken token)
//     {
//         try
//         {
//             await _gateway.Update(new Identified<Domain.Project.Discipline>(id, entity), token);
//         }
//         catch (Exception e)
//         {
//             throw new SaveDisciplineException("Failed to update discipline.", e);
//         }
//     }
// }
//
// public class SaveDisciplineException : Exception
// {
//     internal SaveDisciplineException(string msg) : base(msg)
//     {
//     }
//
//     internal SaveDisciplineException(string msg, Exception innerException) : base
//         (msg, innerException)
//     {
//     }
// }