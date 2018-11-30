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
    public class EditQuestionViewModel :ViewModelBase
    {
        private QuestionVM _question;
        public QuestionVM Question { get
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
        public RelayCommand SaveQuestionChangesCommand { get; set; }
        public RelayCommand<Window> DiscardQuestionChangesCommand { get; set; }
        public EditQuestionViewModel(QuestionVM question)
        {
            _context = new CustomFSContext();
            var questionEntity = _context.Questions.ToList().Where(e => e.Id == question.Id).FirstOrDefault();
            //the QuestionVM question is passed through the constructor, but this entity is not observed by the current context
            //so changes made here do not register to the current context, only to the previous.
            //This means that changint the VM and calling savechanges won't register it as changed and therefore not
            //update the database.
            _question = new QuestionVM(questionEntity);
            var temp = _context.QuestionTypes.ToList().Select(e => new QuestionTypeVM(e));
            QuestionTypes = new ObservableCollection<QuestionTypeVM>(temp);
            SaveQuestionChangesCommand = new RelayCommand(SaveQuestionChanges);
            DiscardQuestionChangesCommand = new RelayCommand<Window>(DiscardQuestionChanges);
            SelectedQuestionType = QuestionTypes.FirstOrDefault();
        }

        public void SaveQuestionChanges()
        {
            MessageBoxResult result = MessageBox.Show("Opslaan?", "Bevestig", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) 
            {
                _context.SaveChanges();
                Messenger.Default.Send(true, "UpdateQuestions");
            }

        }

        public void DiscardQuestionChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Aanpassing annuleren?", "Bevestig", MessageBoxButton.OKCancel);
            //Must close the window it's opened upon confirmation
            //no need to discard changes as the context is separate from the previous window's.
        }
    }
}
