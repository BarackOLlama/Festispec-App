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

        public string Columns
        {
            get { return _question.Columns; }
            set { _question.Columns = value; }
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
