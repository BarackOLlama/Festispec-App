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
        public InspectionVM SelectedInspection { get; set; }

        public RelayCommand ShowCreateEditInspectionViewCommand { get; set; }
        public RelayCommand<Window> BackHomeCommand { get; set; }

        public InspectionManagementViewModel()
        {            
            _Context = new CustomFSContext();
            Inspections = _Context.InspectionCrud.GetInspections();

            ShowCreateEditInspectionViewCommand = new RelayCommand(ShowCreateEditInspectionView);
            BackHomeCommand = new RelayCommand<Window>(CloseAction);
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        private void ShowCreateEditInspectionView()
        {
            new CreateEditInspectionView().Show();                                                                                                                                                                                 
        }
    }
}
