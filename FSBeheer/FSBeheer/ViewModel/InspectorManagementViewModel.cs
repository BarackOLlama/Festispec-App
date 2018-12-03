using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class InspectorManagementViewModel : ViewModelBase
    {

        private CustomFSContext _customFSContext;
        private InspectorVM _selectedInspector;
        public ObservableCollection<InspectorVM> Inspectors { get; set; }
        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand ShowEditInspectorViewCommand { get; set; }
        public RelayCommand ShowCreateInspectorViewCommand { get; set; }
        public RelayCommand DeleteInspectorCommand { get; set; }


        public InspectorManagementViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateInspectorList", il => Init()); // registratie, ontvangt (recipient is dit zelf) Observable Collection van CustomerVM en token is CustomerList, en voeren uiteindelijk init() uit, stap I

            Init();
            
            
            BackHomeCommand = new RelayCommand<Window>(CloseAction);
            ShowEditInspectorViewCommand = new RelayCommand(ShowEditInspectorView);
            ShowCreateInspectorViewCommand = new RelayCommand(ShowCreateInspectorView);
            DeleteInspectorCommand = new RelayCommand(DeleteInspector);
        }

        private void Init()
        {
            _customFSContext = new CustomFSContext();
            Inspectors = _customFSContext.InspectorCrud.GetAllInspectors();
            RaisePropertyChanged(nameof(Inspectors));
        }

        public InspectorVM SelectedInspector
        {
            get
            {
                return _selectedInspector;
            }
            set
            {
                _selectedInspector = value;
                base.RaisePropertyChanged("SelectedInspector");
            }
        }

        private void ShowCreateInspectorView()
        {
            SelectedInspector = null;
            new CreateEditInspectorView().Show();
        }

        private void ShowEditInspectorView()
        {
            new CreateEditInspectorView(SelectedInspector).Show();
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        private void DeleteInspector()
        {
            MessageBoxResult result = MessageBox.Show("Delete the selected inspector?", "Confirm Delete", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                SelectedInspector.IsDeleted = true;
                _customFSContext.SaveChanges();
                

                Messenger.Default.Send(true, "UpdateInspectorList");
            }
        }
    }
}
