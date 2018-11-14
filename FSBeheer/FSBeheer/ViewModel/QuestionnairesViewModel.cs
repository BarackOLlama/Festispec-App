using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class QuestionnairesViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; set; }
        private QuestionnaireVM _selectedQuestionnaire;
        public QuestionnaireVM SelectedQuestionnaire
        {
            get
            {
                return _selectedQuestionnaire;
            }
            set
            {
                _selectedQuestionnaire = value;
                base.RaisePropertyChanged("SelectedQuestionnaire");
            }
        }

        public QuestionnairesViewModel()
        {
            using (var context = new FSContext())
            {
                var result = context.Questionnaires.ToList().Select(e => new QuestionnaireVM(e));
                Questionnaires = new ObservableCollection<QuestionnaireVM>(result);
            }
        }
    }
}
