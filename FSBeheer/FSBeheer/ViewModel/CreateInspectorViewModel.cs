using FSBeheer.VM;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace FSBeheer.ViewModel
{
    public partial class CreateInspectorViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        private ObservableCollection<InspectorVM> _inspectors;

        private InspectorVM _selectedInspectorVM;

        private InspectorVM _inspectorVM;
        public InspectorVM Inspector
        {
            get
            {
                return _inspectorVM;
            }
            set
            {
                _inspectorVM = value;
                base.RaisePropertyChanged("Inspector");
            }
        }


        public CreateInspectorViewModel()
        {
            _Context = new CustomFSContext();
            _inspectors = _Context.InspectorCrud.GetInspectors();
            _inspectorVM = new InspectorVM();
        }
    }
}
