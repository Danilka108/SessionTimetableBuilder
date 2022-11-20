using System;
using App.Project.BellTimeEditor;
using App.Project.ExplorerList;
using Avalonia.Controls;

namespace App.Project.BellTimes;

public class BellTimesView : ExplorerListView
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new BellTimeEditorWindow
        {
            DataContext = viewModel
        };
    }
}