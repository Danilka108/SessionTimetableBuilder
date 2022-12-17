using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.Project.Browser;
using App.Project.TeacherEditor;
using Domain;
using Domain.Project.Models;
using Domain.Project.useCases.Teacher;
using ReactiveUI;

namespace App.Project.TeacherCard;

public class TeacherCardViewModel : ViewModelBase
{
    public delegate TeacherCardViewModel
        Factory(IdentifiedModel<Teacher> teacher, IBrowser browser);

    private readonly DeleteTeacherUseCase _deleteUseCase;

    private TeacherEditorViewModel? _editorViewModel;

    private readonly TeacherEditorViewModel.Factory _editorViewModelFactory;

    private readonly IBrowser _browser;

    private readonly IdentifiedModel<Teacher> _teacher;

    public TeacherCardViewModel(IdentifiedModel<Teacher> teacher, IBrowser browser,
        TeacherEditorViewModel.Factory editorViewModelFactory, DeleteTeacherUseCase deleteUseCase)
    {
        _teacher = teacher;
        _browser = browser;
        _editorViewModelFactory = editorViewModelFactory;
        _deleteUseCase = deleteUseCase;
        Title = teacher.Model.FullName;
        ConfirmDeleteMessage = $"Delete teacher '{Title}'?";

        Open = ReactiveCommand.CreateFromTask(DoOpen);
        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    private async Task DoOpen()
    {
        _editorViewModel = _editorViewModelFactory.Invoke(_teacher, _browser);
        await _browser.Manager.Browse.Execute(_editorViewModel);
    }

    private async Task DoDelete()
    {
        if (_editorViewModel is not null)
        {
            await _browser.Manager.Close.Execute(_editorViewModel);
        }

        await _deleteUseCase.Handle(_teacher.Id);
    }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public ReactiveCommand<Unit, Unit> Open { get; }

    public string ConfirmDeleteMessage { get; }

    public string Title { get; }
}