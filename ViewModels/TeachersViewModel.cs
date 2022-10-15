using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Avalonia.SessionTimetableBuilder.ViewModels;

using System;

public class TeachersViewModel : ViewModelBase
{
    public TeachersViewModel()
    {
        Items = new ObservableCollection<string>(new [] { "Hello" });
    }
    public ObservableCollection<String> Items { get; }
}