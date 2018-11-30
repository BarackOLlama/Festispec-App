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
using System.Windows;

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
        public QuestionVM SelectedQuestion
        {
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
        public RelayCommand DeleteQuestionCommand { get; set; }

        public EditQuestionnaireViewModel(QuestionnaireVM questionnaire)
        {
            Messenger.Default.Register<bool>(this, "UpdateQuestions", cl => Init());
            _context = new CustomFSContext();
            var questionnaireEntity = _context.Questionnaires.ToList().Where(e => e.Id == questionnaire.Id).FirstOrDefault();
            _questionnaire = new QuestionnaireVM(questionnaireEntity);
            Init();

            var questions = _context.Questions
                .Include("QuestionType")
                .ToList()
                .Where(e => e.QuestionnaireId == _questionnaire.Id && !e.IsDeleted)
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
            DeleteQuestionCommand = new RelayCommand(DeleteQuestion);
            SelectedQuestion = questions.FirstOrDefault();
        }

        internal void Init()
        {
            _context = new CustomFSContext();
            var questions = _context.Questions
                .ToList()
                .Where(e=> e.QuestionnaireId == _questionnaire.Id && !e.IsDeleted)
                .Select(e => new QuestionVM(e));
            Questions = new ObservableCollection<QuestionVM>(questions);
            base.RaisePropertyChanged("Questions");
        }


        private void SaveQuestionnaireChanges()
        {
            MessageBoxResult result = MessageBox.Show("Opslaan?", "Bevestig", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.SaveChanges();
                Messenger.Default.Send(true, "UpdateQuestionnaires");
            }
        }

        public void OpenCreateQuestionView()
        {
            new CreateQuestionView().ShowDialog();
        }

        private void OpenEditQuestionView()
        {
            if (_selectedQuestion == null)
            {
                MessageBox.Show("Geen vraag geselecteerd.");
            }
            else
            {
                new EditQuestionView().ShowDialog();
            }
        }

        public void UpdateQuestions()
        {
            var questions = _context.Questions.ToList().Select(e => new QuestionVM(e));
            Questions = new ObservableCollection<QuestionVM>(questions);
        }

        public void DeleteQuestion()
        {
            if (_selectedQuestion == null || _selectedQuestion.IsDeleted)
            {
                MessageBox.Show("Geen vraag geselecteerd.");
            }else
            {
                var result = MessageBox.Show("Vraag verwijderen?", "Verwijder", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _selectedQuestion.IsDeleted = true;
                    _context.SaveChanges();
                    this.Init();
                }
            }
        }
    }
}