using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class InspectorManagementViewModel
    {

        private CustomFSContext _Context;
        public ObservableCollection<InspectorVM> Inspectors { get; }
        public RelayCommand CloseWindowCommand;

        public InspectorManagementViewModel()
        {
            _Context = new CustomFSContext();
            Inspectors = _Context.InspectorCrud.GetInspectors();
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow()
        {
            //TODO: get reference to an open inspectormanagement window through mainviewmodel or other way?
        }
    }
}
