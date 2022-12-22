using System;
using Adapters.Project.ViewModels;
using Adapters.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Infrastructure.Project.Views;

namespace Infrastructure;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        // var name = data.GetType()
        //     .FullName!.Replace("ViewModel", "View")
        //     .Replace("Infrastructure", "Adapters");
        //
        // var type = Type.GetType(name);
        //
        // if (type != null) return (Control)Activator.CreateInstance(type)!;
        //
        // return new TextBlock { Text = "Not Found: " + name };

        return data switch
        {
            ClassroomFeatureCardViewModel context => new ClassroomFeatureCardView
            {
                DataContext = context
            },
            _ => throw new ArgumentNullException(nameof(data)),
        };
    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}