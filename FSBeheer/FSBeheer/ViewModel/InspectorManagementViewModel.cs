using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private CustomFSContext _Context;
        private InspectorVM _selectedInspector;
        public ObservableCollection<InspectorVM> Inspectors { get; }
        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand ShowEditInspectorViewCommand { get; set; }
        public RelayCommand ShowCreateInspectorViewCommand { get; set; }


        public InspectorManagementViewModel()
        {
            _Context = new CustomFSContext();
            Inspectors = _Context.InspectorCrud.GetInspectors();
            BackHomeCommand = new RelayCommand<Window>(CloseAction);
            ShowEditInspectorViewCommand = new RelayCommand(ShowEditInspectorView);
            ShowCreateInspectorViewCommand = new RelayCommand(ShowCreateInspectorView);
            SelectedInspector = Inspectors?.First();
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
            new CreateEditInspectorView().Show();
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

    }
}
