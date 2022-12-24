using System;
using System.Linq;
using Adapters.Project.ViewModels;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using DynamicData;

namespace Infrastructure.Project;

public class ExploredSetExtension : MarkupExtension
{
    public override string[] ProvideValue(IServiceProvider serviceProvider)
    {
        var exploredSets = Enum
            .GetValues<ExploredSet>();

        var convertedExploredSets = new string[exploredSets.Length];

        for (var i = 0; i < exploredSets.Length; i++)
        {
            convertedExploredSets[i] = exploredSets[i] switch
            {
                ExploredSet.Classrooms => "ClassroomsTitle",
                ExploredSet.ClassroomFeatures => "ClassroomsFeaturesTitle",
                ExploredSet.Disciplines => "DisciplinesTitle"
            };
        }

        return convertedExploredSets;
    }
}