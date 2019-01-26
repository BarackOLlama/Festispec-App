using FSBeheer.Model;
using System.Collections.ObjectModel;

namespace FSBeheer.VM
{
    public class QuestionVM
    {
        private Question _question;

        public QuestionVM(Question question)
        {
            _question = question;
        }

        public QuestionVM()
        {
            _question = new Question();
            _question.IsDeleted = false;
        }

        public int Id
        {
            get { return _question.Id; }
        }

        public int? QuestionnaireId
        {
            get { return _question.QuestionnaireId; }
            set
            {
                int? conv = value;
                _question.QuestionnaireId = conv;
            }
        }

        public string Content
        {
            get { return _question.Content; }
            set { _question.Content = value; }
        }

        public string Options
        {
            get { return _question.Options; }
            set { _question.Options = value; }
        }

        public ObservableCollection<Option> OptionsDictionary
        {
            get
            {
                var collection = new ObservableCollection<Option>();
                if (_question.Options != null)
                {
                    foreach (string s in _question.Options.Split(';'))
                    {
                        collection.Add(new Option
                        {
                            Key = s.Split('|')[0],
                            Value = s.Split('|')[1]
                        });
                    }
                } else // scale question
                {
                    // TODO
                    //string letter = "A";
                    //foreach (string s in _question.Scale.Split(':'))
                    //{
                        
                    //    collection.Add(new Option {
                    //        Key = letter,
                    //        Value = s

                    //    });
                        
                    //}
                }
                return collection;
            }
        }

        public string Columns
        {
            get { return _question.Columns; }
            set { _question.Columns = value; }
        }

        public string Scale
        {
            get { return _question.Scale; }
            set { _question.Scale = value; }
        }

        public QuestionType Type
        {
            set { _question.QuestionType = value; }
            get { return _question.QuestionType; }
        }

        public string Comments
        {
            get { return _question.Comments; }
            set { _question.Comments = value; }
        }

        public bool IsDeleted
        {
            get { return _question.IsDeleted; }
            set { _question.IsDeleted = value; }
        }

        internal Question ToModel()
        {
            return _question;
        }

        public override string ToString()
        {
            return _question.QuestionType.Name;
        }

    }

    public class Option
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string OptionString
        {
            get { return Key + "|" + Value; }
        }
    }
}
