using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class EditInspectorViewModel : ViewModelBase
    {

        public InspectorVM Inspector { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }



        private CustomFSContext _Context;

        public EditInspectorViewModel(InspectorVM SelectedInspector)
        {
            _Context = new CustomFSContext();
            EditCommand = new RelayCommand(ModifyInspector);

            if (SelectedInspector != null)
            {
                Inspector = SelectedInspector;

            }
            else
            {
                Inspector = new InspectorVM();

            }
        }

        private void AddInspector()
        {

            _Context.InspectorCrud.GetAllInspectorVMs().Add(Inspector);
            _Context.InspectorCrud.Add(Inspector);
        }


        private void ModifyInspector() => _Context.InspectorCrud.Modify(Inspector);



        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                _Context.Dispose();
                Inspector = null;
                window?.Close();
            }
        }

    }
}
