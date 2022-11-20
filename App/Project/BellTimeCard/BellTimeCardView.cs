using System;
using App.Project.BellTimeEditor;
using App.Project.ExplorerCard;
using Avalonia.Controls;

namespace App.Project.BellTimeCard;

public class BellTimeCardView : ExplorerCardView
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new BellTimeEditorWindow
        {
            DataContext = viewModel
        };
    }
}