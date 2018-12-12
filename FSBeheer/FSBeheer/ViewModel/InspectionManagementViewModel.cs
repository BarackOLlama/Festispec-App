using FSBeheer.VM;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using FSBeheer.View;

namespace FSBeheer.ViewModel
{
    public class InspectionManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; set; }
        private InspectionVM _SelectedInspection { get; set; }
        public InspectionVM SelectedInspection {
            get
            {
                return _SelectedInspection;
            }
            set
            {
                _SelectedInspection = value;
                base.RaisePropertyChanged(nameof(SelectedInspection));
            }
        }

        public RelayCommand ShowEditInspectionViewCommand { get; set; }
        public RelayCommand ShowCreateInspectionViewCommand { get; set; }
        //public RelayCommand<Window> BackHomeCommand { get; set; }

        public InspectionManagementViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateInspectionList", il => Init());
            Init();

            ShowEditInspectionViewCommand = new RelayCommand(ShowEditInspectionView);
            ShowCreateInspectionViewCommand = new RelayCommand(ShowCreateInspectionView);
            //BackHomeCommand = new RelayCommand<Window>(CloseAction);
        }

        internal void Init()
        {
            _Context = new CustomFSContext();
            Inspections = _Context.InspectionCrud.GetAllInspectionVMs();
            RaisePropertyChanged(nameof(Inspections));
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        private void ShowEditInspectionView()
        {
            if (_SelectedInspection == null)
            {
                MessageBox.Show("Er is geen inspectie geselecteerd. Kies een inspectie en kies daarna de optie 'Wijzig'.");
            }
            else
            {
                new CreateEditInspectionView(_SelectedInspection.Id).Show();
            }
        }

        private void ShowCreateInspectionView()
        {
            new CreateEditInspectionView().Show();                                                                                                                                                                                 
        }
    }
}
