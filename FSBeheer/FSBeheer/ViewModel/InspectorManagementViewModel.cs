using FSBeheer.View;
using FSBeheer.VM;
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
    public class InspectorManagementViewModel
    {

        private CustomFSContext _Context;
        public ObservableCollection<InspectorVM> Inspectors { get; }
        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand ShowEditInspectorViewCommand { get; set; }
        public RelayCommand ShowCreateInspectorViewCommand { get; set; }

        public InspectorManagementViewModel()
        {
            _Context = new CustomFSContext();
            Inspectors = _Context.InspectorCrud.GetAllInspectors();
            BackHomeCommand = new RelayCommand<Window>(CloseAction);
            ShowEditInspectorViewCommand = new RelayCommand(ShowEditInspectorView);
            ShowCreateInspectorViewCommand = new RelayCommand(ShowCreateInspectorView);
        }

        private void ShowCreateInspectorView()
        {
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
