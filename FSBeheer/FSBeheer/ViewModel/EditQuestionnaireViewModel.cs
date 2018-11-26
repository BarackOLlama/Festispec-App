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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace FSBeheer.ViewModel
{
    public class EditQuestionnaireViewModel : ViewModelBase
    {
        private QuestionnaireVM _questionnaire;
        private CustomFSContext _context;
        public QuestionnaireVM Questionnaire
        {
            get { return _questionnaire; }
            set
            {
                _questionnaire = value;
                base.RaisePropertyChanged("Questionnaire");
            }
        }

        private QuestionVM _selectedQuestion;
        public QuestionVM SelectedQuestion {
            get
            {
                return _selectedQuestion;
            }
            set
            {
                _selectedQuestion = value;
                base.RaisePropertyChanged("SelectedQuestion");
            }
        }
        public ObservableCollection<int?> InspectionNumbers { get; set; }
        public ObservableCollection<QuestionVM> Questions { get; set; }
        private int? _selectedInspectionNumber;
        public int? SelectedInspectionNumber
        {
            get { return _selectedInspectionNumber; }
            set
            {
                _selectedInspectionNumber = value;
                base.RaisePropertyChanged("SelectedInspectionNumber");
            }
        }

        public RelayCommand OpenCreateQuestionViewCommand { get; set; }
        public RelayCommand SaveQuestionnaireChangesCommand { get; set; }
        public RelayCommand OpenEditQuestionViewCommand { get; set; }

        public EditQuestionnaireViewModel(QuestionnaireVM questionnaire)
        {
            _context = new CustomFSContext();
            _questionnaire = questionnaire;
            var questions = _context.Questions
                .Include("QuestionType")
                .ToList()
                .Where(e => e.QuestionnaireId == _questionnaire.Id)
                .Select(e => new QuestionVM(e));
            Questions = new ObservableCollection<QuestionVM>(questions);

            var inspectionNumbers = _context.Inspections
                .ToList()
                .Where(e => !e.IsDeleted)
                .Select(e => (int?)e.Id);
            InspectionNumbers = new ObservableCollection<int?>(inspectionNumbers);
            _selectedInspectionNumber = inspectionNumbers.FirstOrDefault();

            OpenCreateQuestionViewCommand = new RelayCommand(OpenCreateQuestionView);
            SaveQuestionnaireChangesCommand = new RelayCommand(SaveQuestionnaireChanges);
            OpenEditQuestionViewCommand = new RelayCommand(OpenEditQuestionView);
            SelectedQuestion = questions.FirstOrDefault();
        }


        private void SaveQuestionnaireChanges()
        {
            _context.QuestionnaireCrud.GetAllQuestionnaireVMs().Add(_questionnaire);
            var temp = _context.QuestionnaireCrud.GetAllQuestionnaireVMs();
            _context.SaveChanges();
            //Messenger.Default.Send(true, "UpdateQuestionnaires"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
            //^is used in the CreateEditCustomerViewModel
        }

        public void OpenCreateQuestionView()
        {
            new CreateQuestionView().ShowDialog();
        }

        private void OpenEditQuestionView()
        {
            new EditQuestionView().ShowDialog();
        }

    }
}