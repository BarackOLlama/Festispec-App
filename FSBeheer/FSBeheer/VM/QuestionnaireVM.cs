using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;

namespace FSBeheer.VM
{
    public class QuestionnaireVM : ViewModelBase
    {
        private Questionnaire _questionnaire;
        public ObservableCollection<QuestionVM> Questions { get; set; }

        public QuestionnaireVM(Questionnaire questionnaire)
        {
            _questionnaire = questionnaire;
        }

        public QuestionnaireVM()
        {
            _questionnaire = new Questionnaire();
            _questionnaire.Version = 1;
        }

        public int Id
        {
            get { return _questionnaire.Id; }
            set { _questionnaire.Id = value; }
        }

        public int? InspectionId
        {
            get
            {
                return _questionnaire.InspectionId;
            }
            set
            {
                _questionnaire.InspectionId = value;
                base.RaisePropertyChanged(nameof(InspectionId));
            }
        }

        public string Name
        {
            get
            {
                return _questionnaire.Name;
            }
            set
            {
                _questionnaire.Name = value;
                base.RaisePropertyChanged("Name");
            }
        }

        public int Version
        {
            get
            {
                return _questionnaire.Version;
            }
            set
            {
                _questionnaire.Version = value;
                base.RaisePropertyChanged(nameof(Version));
            }
        }

        public string Instructions
        {
            get
            {
                return _questionnaire.Instructions;
            }
            set
            {
                _questionnaire.Instructions = value;
                base.RaisePropertyChanged("Instructions");
            }
        }

        public string Comments
        {
            get
            {
                return _questionnaire.Comments;
            }
            set
            {
                _questionnaire.Comments = value;
                base.RaisePropertyChanged("Comments");
            }
        }

        public int? QuestionCount
        {
            get
            {
                int? amount = 0;
                amount = _questionnaire?.Questions?.Count();
                return amount;
            }
        }

        public Event Event
        {
            get { return _questionnaire.Inspection?.Event; }
        }

        public InspectionVM Inspection
        {
            get { return new InspectionVM(_questionnaire.Inspection); }
            set { _questionnaire.Inspection = value.ToModel(); }
        }


        public bool IsDeleted
        {
            get { return _questionnaire.IsDeleted; }
            set { _questionnaire.IsDeleted = value; }
        }

        internal Questionnaire ToModel()
        {
            return _questionnaire;
        }
    }
}