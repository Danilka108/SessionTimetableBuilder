using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using App.Views.Teacher;
using DynamicData.Kernel;
using ReactiveUI;

namespace App.Views.Teachers;

public class TeachersViewModel : ViewModelBase
{
    public TeachersViewModel(IEnumerable<Models.Teacher> teachers,
        Func<IObservable<IRoutableViewModel>> createNewTeacher)
    {
        CreateNewTeacherCommand = ReactiveCommand.CreateFromObservable(createNewTeacher);

        TeacherViewModels =
            new ObservableCollection<TeacherViewModel>(
                teachers
                    .AsList()
                    .Select(i => new TeacherViewModel(i))
            );
    }

    public ReactiveCommand<Unit, IRoutableViewModel> CreateNewTeacherCommand { get; }

    public ObservableCollection<TeacherViewModel> TeacherViewModels { get; }
}