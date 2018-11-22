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

namespace FSBeheer.VM
{
    public class QuestionnaireVM
    {
        private Questionnaire _questionnaire;
        public ObservableCollection<QuestionVM> Questions { get; set; }

        public QuestionnaireVM(Questionnaire questionnaire)
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
        }

        public QuestionnaireVM()
        {
            _questionnaire = new Questionnaire();
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
            set
            {
                _questionnaire.Name = value;
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
            }
        }

        internal Questionnaire ToModel()
        {
            return _questionnaire;
        }
    }
}