using FSBeheer.VM;
using FSBeheer.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using FSBeheer.View;

namespace FSBeheer.ViewModel
{
    public class InspectionManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; }

        public RelayCommand ShowEditInspectionViewCommand { get; set; }
        public RelayCommand<Window> BackHomeCommand { get; set; }

        public InspectionManagementViewModel()
        {
            //var questions = _Context.Questions.ToList().Select(q => new QuestionVM(q));
            //Questions = new ObservableCollection<QuestionVM>(questions);

            // klopt deze instantie van CustomFSContext?
            _Context = new CustomFSContext();
            Inspections = _Context.InspectionCrud.GetInspections();
            //RaisePropertyChanged(nameof(Inspections));
            ShowEditInspectionViewCommand = new RelayCommand(ShowEditInspectionView);
            BackHomeCommand = new RelayCommand<Window>(CloseAction);

        }

        private void ShowEditInspectionView()
        {
            new EditInspectionView().Show();
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

    }
}
