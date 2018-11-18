using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public int Id
        {
            get { return _question.Id; }
        }

        public string Content
        {
            get
            {
                return _question.Content;
            }
            set
            {
                _question.Content = value;
            }
        }

        public string Options
        {
            get { return _question.Options; }
        }

        public string Columns
        {
            get { return _question.Columns; }
        }

        public QuestionType Type
        {
            set { _question.QuestionType = value; }
            get { return _question.QuestionType; }
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
}
