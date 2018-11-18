using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace FSBeheer.ViewModel
{
    public class QuestionnaireVM
    {
        private Questionnaire _questionnaire;
        private CreateQuestionView _createQuestionView;
        public ObservableCollection<QuestionVM> Questions { get; set; }

        public ICommand OpenCreateQuestionCommand { get; set; }

        public QuestionnaireVM(Questionnaire questionnaire)
        {
            _questionnaire = questionnaire;

            using (var context = new FSContext())
            {
                var questions = context.Questions
                    .Include("QuestionType")
                    .ToList()
                    .Where(e => e.QuestionnaireId == _questionnaire.Id)
                    .Select(e => new QuestionVM(e));
                Questions = new ObservableCollection<QuestionVM>(questions);
            }

            Question q = new Model.Question();
            OpenCreateQuestionCommand = new RelayCommand(OpenCreateQuestionWindow);
        }

        public int Id
        {
            get
            {
                return _questionnaire.Id;
            }
        }

        public string Name
        {
            get
            {
                return _questionnaire.Name;
            }
        }

        public void OpenCreateQuestionWindow()
        {
            new CreateQuestionView().ShowDialog();
        }
    }
}