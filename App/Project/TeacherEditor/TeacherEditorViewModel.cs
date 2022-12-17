using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using App.Project.Browser;
using Avalonia.Controls.Selection;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases.Discipline;
using Domain.Project.useCases.Teacher;
using DynamicData;
using ReactiveUI;

namespace App.Project.TeacherEditor;

public class TeacherEditorViewModel : ViewModelBase, IBrowserPage, IActivatableViewModel
{
    public delegate TeacherEditorViewModel Factory(IdentifiedModel<Teacher>? teacher,
        IBrowser browser);

    private string _name;

    private string _surname;

    private string _patronymic;

    private string _pageName;

    private readonly int? _id;

    private readonly ObserveAllDisciplinesUseCase _observeAllDisciplinesUseCase;

    public TeacherEditorViewModel(
        IdentifiedModel<Teacher>? teacher,
        IBrowser browser,
        ObserveAllDisciplinesUseCase observeDisciplinesUseCase
    )
    {
        Activator = new ViewModelActivator();
        SelectedDisciplines = new ObservableCollection<IdentifiedModel<Discipline>>(
            teacher?.Model.AcceptedDisciplines ?? new IdentifiedModel<Discipline>[] { });
        AllDisciplines = new ObservableCollection<IdentifiedModel<Discipline>>();
        
        _observeAllDisciplinesUseCase = observeDisciplinesUseCase;
        
        observeDisciplinesUseCase
            .Handle()
            .Subscribe(IntersectAllDisciplinesWith);

        _id = teacher?.Id;
        Name = teacher?.Model.Name ?? "";
        Surname = teacher?.Model.Surname ?? "";
        Patronymic = teacher?.Model.Surname ?? "";
    }

    private void IntersectAllDisciplinesWith(IEnumerable<IdentifiedModel<Discipline>> updatedDisciplines)
    {
        var comparer = new IdentifiedModel<Discipline>.EqualityComparer();

        var itemsToRemove = from oldDiscipline in AllDisciplines
            let doesNotContains = !updatedDisciplines.Contains(oldDiscipline, comparer)
            where doesNotContains
            select oldDiscipline;

        AllDisciplines.RemoveMany(itemsToRemove);

        var disciplinesToAdd =
            from newDiscipline in updatedDisciplines
            let doesNotContains = !AllDisciplines.Contains(newDiscipline, comparer)
            where doesNotContains
            select newDiscipline;

        AllDisciplines.AddRange(disciplinesToAdd);
    }

    public string PageName
    {
        get => _pageName;
        private set => this.RaiseAndSetIfChanged(ref _pageName, value);
    }

    public string Name
    {
        get => _name;
        set
        {
            this.RaiseAndSetIfChanged(ref _name, value);
            PageName = Teacher.ConvertToFullName(Name, Surname, Patronymic);
        }
    }

    public string Surname
    {
        get => _surname;
        set
        {
            this.RaiseAndSetIfChanged(ref _surname, value);
            PageName = Teacher.ConvertToFullName(Name, Surname, Patronymic);
        }
    }

    public string Patronymic
    {
        get => _patronymic;
        set
        {
            this.RaiseAndSetIfChanged(ref _patronymic, value);
            PageName = Teacher.ConvertToFullName(Name, Surname, Patronymic);
        }
    }

    public ObservableCollection<IdentifiedModel<Discipline>> AllDisciplines { get; }

    private ObservableCollection<IdentifiedModel<Discipline>> SelectedDisciplines { get; }

    public ViewModelActivator Activator { get; }
}