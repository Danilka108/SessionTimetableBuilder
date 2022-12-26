using System;
using Adapters;
using Adapters.Project.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Infrastructure.Project.Views;

namespace Infrastructure.Project;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        return data switch
        {
            ClassroomFeatureCardViewModel context => new ClassroomFeatureCardView
            {
                ViewModel = context
            },
            ClassroomCardViewModel context => new ClassroomCardView
            {
                ViewModel = context
            },
            DisciplineCardViewModel context => new DisciplineCardView
            {
                ViewModel = context
            },
            LecturerCardViewModel context => new LecturerCardView
            {
                ViewModel = context
            },
            GroupCardViewModel context => new GroupCardView
            {
                ViewModel = context
            },
            ExamCardViewModel context => new ExamCardView
            {
                ViewModel = context
            },
            _ => throw new ArgumentNullException(nameof(data))
        };
    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}