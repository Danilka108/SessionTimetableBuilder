using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace App;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        var dataType = data.GetType();

        
        
        var name = data.GetType()
            .FullName!.Replace("ViewModel", "View");

        var type = Type.GetType(name);

        if (type != null) return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}