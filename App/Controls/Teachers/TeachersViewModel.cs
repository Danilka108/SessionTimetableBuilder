using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App.Controls.Teacher;
using DynamicData.Kernel;

namespace App.Controls.Teachers;

public class TeachersViewModel : ViewModelBase
{
    public TeachersViewModel(IEnumerable<Models.Teacher> teachers)
    {
        TeacherViewModels =
            new ObservableCollection<TeacherViewModel>(
                teachers
                    .AsList()
                    .Select(i => new TeacherViewModel(i))
            );
    }

    public ObservableCollection<TeacherViewModel> TeacherViewModels { get; }
}