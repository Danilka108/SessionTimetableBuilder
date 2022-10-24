using System.Collections.ObjectModel;
using System.Linq;
using Domain.Models;

namespace App.ViewModels;

public class TeachersViewModel : ViewModelBase
{
    readonly ClassTimeBounds[] ClassesTileBounds = new[]
    {
        new ClassTimeBounds(1, new BellTime(8, 30), new BellTime(10, 00)),
        new ClassTimeBounds(2, new BellTime(10, 15), new BellTime(11, 45)),
        new ClassTimeBounds(3, new BellTime(12, 00), new BellTime(13, 30)),
        new ClassTimeBounds(4, new BellTime(14, 00), new BellTime(15, 30)),
        new ClassTimeBounds(5, new BellTime(15, 45), new BellTime(17, 15)),
        new ClassTimeBounds(6, new BellTime(17, 30), new BellTime(19, 00)),
        new ClassTimeBounds(7, new BellTime(19, 15), new BellTime(20, 45)),
        new ClassTimeBounds(8, new BellTime(21, 00), new BellTime(22, 30)),
    };

    public TeachersViewModel()
    {
        Items = new ObservableCollection<Teacher>(new[]
            {
                new Teacher
                (
                    "Попов",
                    "Евгений",
                    "Александрович",
                    ClassesTileBounds.ToList().FindAll(e => e.Number % 2 == 0)
                ),
                new Teacher
                (
                    "Левяков",
                    "Станислав",
                    "Вячеславович",
                    ClassesTileBounds.ToList().FindAll(e => e.Number % 2 != 0)
                ),
            }
        );
    }

    public ObservableCollection<Teacher> Items { get; }
}