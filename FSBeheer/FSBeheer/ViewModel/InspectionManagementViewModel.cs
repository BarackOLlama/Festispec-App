using FSBeheer.VM;
using FSBeheer.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FSBeheer.ViewModel
{
    public class InspectionManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; }

        public RelayCommand ShowEditInspectionViewCommand { get; set; }
        public RelayCommand<Window> BackHomeCommand { get; set; }

        public RelayCommand AddNewInspectionCommand;

        public InspectionManagementViewModel()
        {            
            _Context = new CustomFSContext();
            Inspections = _Context.InspectionCrud.GetInspections();
            //RaisePropertyChanged(nameof(Inspections));
            ShowEditInspectionViewCommand = new RelayCommand(ShowEditInspectionView);
            BackHomeCommand = new RelayCommand<Window>(CloseAction);


            AddNewInspectionCommand = new RelayCommand(AddNewInspection);
        }

        private void ShowEditInspectionView()
        {
            new EditInspectionView().Show();
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }
        private void AddNewInspection()
        {

        }
    }
}
