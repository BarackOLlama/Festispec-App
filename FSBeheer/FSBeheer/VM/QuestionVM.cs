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
        private bool e;

        public QuestionVM(Question question)
        {
            _question = question;
        }

        public int Id
        {
            get { return _question.Id; }
        }

        public string Content
        {
            get { return _question.Content; }
        }

        public string Options
        {
            get { return _question.Options; }
        }

        public string Columns
        {
            get { return _question.Columns; }
        }

        public string Type
        {
            get { return _question.QuestionType.Name; }
        }

        internal Question ToModel()
        {
            return _question;
        }
    }
}
