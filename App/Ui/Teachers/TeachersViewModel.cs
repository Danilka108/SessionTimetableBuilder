using System.Collections.ObjectModel;
using Domain.Models;

namespace App.ViewModels;

public class TeachersViewModel : ViewModelBase
{
    private readonly ClassTimeBounds[] _classesTileBounds =
    {
        new(1, new BellTime(8, 30), new BellTime(10, 00)),
        new(2, new BellTime(10, 15), new BellTime(11, 45)),
        new(3, new BellTime(12, 00), new BellTime(13, 30)),
        new(4, new BellTime(14, 00), new BellTime(15, 30)),
        new(5, new BellTime(15, 45), new BellTime(17, 15)),
        new(6, new BellTime(17, 30), new BellTime(19, 00)),
        new(7, new BellTime(19, 15), new BellTime(20, 45)),
        new(8, new BellTime(21, 00), new BellTime(22, 30))
    };

    public ObservableCollection<Teacher> Items { get; }
}