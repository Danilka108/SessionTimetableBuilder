using System;
using Adapters;
using Adapters.Project.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Infrastructure.Project.Views;

namespace Infrastructure;

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
            _ => throw new ArgumentNullException(nameof(data))
        };
    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}