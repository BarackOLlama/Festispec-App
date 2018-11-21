using FSBeheer.Model;
using FSBeheer.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FSBeheer.ViewModel
{
    public class QuestionnairesViewModel : ViewModelBase
    {
        //PLURAL FORM
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; set; }
        private QuestionnaireVM _selectedQuestionnaire;

        private QuestionnaireEditView _questionnaireEditView;

        public ICommand ViewQuestionnaireCommand { get; set; }

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
                var result = context.Questionnaires.Include("Questions").ToList().Select(e => new QuestionnaireVM(e));
                Questionnaires = new ObservableCollection<QuestionnaireVM>(result);
            }
            SelectedQuestionnaire = Questionnaires?.First();
            ViewQuestionnaireCommand = new RelayCommand(OpenCreateQuestionWindow);
        }


        public void OpenCreateQuestionWindow()
        {
            _questionnaireEditView = new QuestionnaireEditView();
            _questionnaireEditView.Show();
        }
    }
}
