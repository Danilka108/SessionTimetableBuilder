// using System.Reactive.Linq;
// using Application.Project.Gateways;
//
// namespace Application.Project.UseCases.ClassroomFeature;
//
// public class ObserveAllClassroomFeaturesUseCase
// {
//     private readonly IClassroomFeatureGateway _featureGateway;
//
//     public ObserveAllClassroomFeaturesUseCase
//         (IClassroomFeatureGateway featureGateway)
//     {
//         _featureGateway = featureGateway;
//     }
//
//     public IObservable<IEnumerable<Identified<Domain.Project.ClassroomFeature>>> Handle()
//     {
//         return _featureGateway.ObserveAll()
//             .Catch<IEnumerable<Identified<Domain.Project.ClassroomFeature>>, Exception>
//             (
//                 e => Observable.Throw<IEnumerable<Identified<Domain.Project.ClassroomFeature>>>
//                 (
//                     new ObserveAllClassroomFeaturesException
//                         ("Failed to get all classroom features", e)
//                 )
//             );
//     }
// }
//
// public class ObserveAllClassroomFeaturesException : Exception
// {
//     internal ObserveAllClassroomFeaturesException(string msg, Exception innerException) : base
//         (msg, innerException)
//     {
//     }
// }