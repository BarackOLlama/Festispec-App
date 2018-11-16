using FSBeheer.VM;
using FSBeheer.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace FSBeheer.ViewModel
{
    public class InspectionManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; }

        public InspectionManagementViewModel()
        {
            //var questions = _Context.Questions.ToList().Select(q => new QuestionVM(q));
            //Questions = new ObservableCollection<QuestionVM>(questions);

            // klopt deze instantie van CustomFSContext?
            _Context = new CustomFSContext();
            Inspections = _Context.GetInspections();
            //RaisePropertyChanged(nameof(Inspections));
        }

        

    }
}
