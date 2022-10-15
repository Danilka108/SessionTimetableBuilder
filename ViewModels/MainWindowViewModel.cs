using System;
using System.Collections.ObjectModel;

namespace Avalonia.SessionTimetableBuilder.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            TeachersViewModel = new TeachersViewModel();
        }
        
        public TeachersViewModel TeachersViewModel { set; get; }
    }
}