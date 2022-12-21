using System;
using System.Linq;
using Adapters.Project.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;

namespace Infrastructure.Project;

public class ExploredSetConverter : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var exploredSets = Enum
            .GetValues<ExploredSet>();

        var convertedExploredSets = new string[exploredSets.Length];

        for (var i = 0; i < exploredSets.Length; i++)
        {
            if (exploredSets[i] == ExploredSet.ClassroomFeature)
            {
                convertedExploredSets[i] = "Classroom feature";
            } 
        }

        return convertedExploredSets.Select(s => new ComboBoxItem
        {
            Content = new TextBlock
            {
                Text = s
            }
        });
    }
}