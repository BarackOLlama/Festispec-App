using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;

namespace FSBeheer.ViewModel
{
    public class QuestionnaireViewModel
    {
        private QuestionnaireVM _questionnaire;
        public ObservableCollection<QuestionVM> Questions { get; set; }

        public RelayCommand OpenCreateQuestionViewCommand { get; set; }

        public QuestionnaireViewModel(QuestionnaireVM questionnaire)
        {
            _questionnaire = questionnaire;

            using (var context = new CustomFSContext())
            {
                var questions = context.Questions
                    .Include("QuestionType")
                    .ToList()
                    .Where(e => e.QuestionnaireId == _questionnaire.Id)
                    .Select(e => new QuestionVM(e));
                Questions = new ObservableCollection<QuestionVM>(questions);
            }

            OpenCreateQuestionViewCommand = new RelayCommand(OpenCreateQuestionView);
        }

        public void OpenCreateQuestionView()
        {
            new CreateQuestionView().ShowDialog();
        }
    }
}