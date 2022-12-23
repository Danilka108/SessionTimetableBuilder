using System.Collections;
using System.Reactive.Linq;
using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

// public class ObserveAllDisciplinesUseCase
// {
//     private readonly IDisciplineGateway _gateway;
//
//     public ObserveAllDisciplinesUseCase(IDisciplineGateway gateway)
//     {
//         _gateway = gateway;
//     }
//
//     public IObservable<IEnumerable<Identified<Domain.Project.Discipline>>> Handle()
//     {
//         return _gateway
//             .ObserveAll()
//             .Catch<IEnumerable<Identified<Domain.Project.Discipline>>, Exception>
//             (
//                 e => Observable.Throw<IEnumerable<Identified<Domain.Project.Discipline>>>
//                     (new ObserveAllDisciplinesException("Failed to get all disciplines.", e))
//             );
//     }
// }
//
// public class ObserveAllDisciplinesException : Exception
// {
//     internal ObserveAllDisciplinesException(string msg, Exception innerException) : base
//         (msg, innerException)
//     {
//     }
// }