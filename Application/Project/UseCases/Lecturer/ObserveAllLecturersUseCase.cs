namespace Application.Project.useCases.Lecturer;

// public class ObserveAllLecturersUseCase
// {
//     private readonly ILecturerGateway _gateway;
//
//     public ObserveAllLecturersUseCase(ILecturerGateway gateway)
//     {
//         _gateway = gateway;
//     }
//
//     public IObservable<IEnumerable<Identified<Domain.Project.Lecturer>>> Handle()
//     {
//         return _gateway
//             .ObserveAll()
//             .Catch<IEnumerable<Identified<Domain.Project.Lecturer>>, Exception>
//             (
//                 e => Observable.Throw<IEnumerable<Identified<Domain.Project.Lecturer>>>
//                     (new ObserveAllLecturersException("Failed to get all lecturers.", e))
//             );
//     }
// }
//
// public class ObserveAllLecturersException : Exception
// {
//     internal ObserveAllLecturersException(string msg, Exception innerException) : base
//         (msg, innerException)
//     {
//     }
// }