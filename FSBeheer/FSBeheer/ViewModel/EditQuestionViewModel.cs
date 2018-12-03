using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    /// <summary>
    /// The responsibility of this class and the view is to edit questions in the database.
    /// It is not responsible for deletion of questions.
    /// </summary>
    public class EditQuestionViewModel : ViewModelBase
    {
        private QuestionVM _question;
        public QuestionVM Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                base.RaisePropertyChanged("Question");
            }
        }
        private QuestionTypeVM _questionType;
        public QuestionTypeVM SelectedQuestionType
        {
            get
            {
                return _questionType;
            }
            set
            {
                _questionType = value;
                base.RaisePropertyChanged("QuestionType");
            }
        }

        private CustomFSContext _context;
        public ObservableCollection<QuestionTypeVM> QuestionTypes { get; set; }
        public RelayCommand<Window> SaveQuestionChangesCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; set; }
        public EditQuestionViewModel(int questionId)
        {
            _context = new CustomFSContext();
            var questionEntity = _context.Questions
                .ToList()
                .Where(e => e.Id == questionId)
                .FirstOrDefault();
            _question = new QuestionVM(questionEntity);

            var types = _context.QuestionTypes
                .ToList()
                .Select(e => new QuestionTypeVM(e));
            QuestionTypes = new ObservableCollection<QuestionTypeVM>(types);

            SaveQuestionChangesCommand = new RelayCommand<Window>(SaveQuestionChanges);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            SelectedQuestionType = QuestionTypes.FirstOrDefault();
        }

        public void SaveQuestionChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Opslaan wijzigingen?", "Bevestiging", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) 
            {
                _context.SaveChanges();
                Messenger.Default.Send(true, "UpdateQuestions");
                window.Close();
            }

        }

        public void CloseWindow(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Aanpassing annuleren?", "Bevestig", MessageBoxButton.OKCancel);
            if(result == MessageBoxResult.OK)
            {
                window.Close();
            }
            //Must close the window it's opened upon confirmation
            //no need to discard changes as the context is separate from the previous window's.
        }
    }
}
