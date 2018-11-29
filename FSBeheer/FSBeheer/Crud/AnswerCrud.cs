using FSBeheer.VM;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace FSBeheer.Crud
{
    class AnswerCrud : AbstractCrud
    {
        public AnswerCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        /**
         * This method needs to be edited and tested, used .ToString() without testing.
         */
        public ObservableCollection<AnswerVM> GetAllAnswersFiltered(string must_contain)
        {
            if (must_contain == null)
            {
                throw new ArgumentNullException(nameof(must_contain));
            }
            must_contain = must_contain.ToLower();

            var _answers = CustomFSContext.Answers
                .ToList()
                .Where(a =>
                a.Id.ToString().ToLower().Contains(must_contain) ||
                a.Content.Contains(must_contain) ||
                a.Inspector.Name.ToLower().Contains(must_contain) ||
                a.Question.Content.ToLower().Contains(must_contain)
                ).Distinct()
                .Select(a => new AnswerVM(a));
            return new ObservableCollection<AnswerVM>(_answers);
        }
        /*
         * Returns all AnswerVMs
         */

        public ObservableCollection<AnswerVM> GetAllAnswers()
        {
            var _result = CustomFSContext.Answers
               .ToList()
               .Select(c => new AnswerVM(c));
            var result = new ObservableCollection<AnswerVM>(_result);

            return result;
        }


        /*
         * Returns one AnswerVM based on ID
         */
        public ObservableCollection<AnswerVM> GetAnswerById(int customer_id)
        {
            var _answer = CustomFSContext.Answers
               .ToList()
               .Where(c => c.Id == customer_id)
               .Select(c => new AnswerVM(c));
            var answers = new ObservableCollection<AnswerVM>(_answer);

            return answers;
        }

        public void Add(AnswerVM _answer) => CustomFSContext.Answers.Add(_answer.ToModel());

        public void Modify(AnswerVM _answer)
        {
            // SelectedCustomer
            CustomFSContext.Entry(_answer?.ToModel()).State = EntityState.Modified;
            CustomFSContext.SaveChanges();
        }

        public void Delete(AnswerVM _answer)
        {
            CustomFSContext.Answers.Attach(_answer?.ToModel());
            CustomFSContext.Answers.Remove(_answer?.ToModel());
            CustomFSContext.SaveChanges();
        }
    }
}
