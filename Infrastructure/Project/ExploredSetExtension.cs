using System;
using Adapters.Project.ViewModels;
using Avalonia.Markup.Xaml;

namespace Infrastructure.Project;

public class ExploredSetExtension : MarkupExtension
{
    public override string[] ProvideValue(IServiceProvider serviceProvider)
    {
        var exploredSets = Enum
            .GetValues<ExploredSet>();

        var convertedExploredSets = new string[exploredSets.Length];

        for (var i = 0; i < exploredSets.Length; i++)
            convertedExploredSets[i] = exploredSets[i] switch
            {
                ExploredSet.Classrooms => "ClassroomsTitle",
                ExploredSet.ClassroomFeatures => "ClassroomsFeaturesTitle",
                ExploredSet.Disciplines => "DisciplinesTitle",
                ExploredSet.Lecturers => "LecturersTitle",
                ExploredSet.Groups => "GroupsTitle",
                ExploredSet.Exams => "ExamsTitle"
            };

        return convertedExploredSets;
    }
}