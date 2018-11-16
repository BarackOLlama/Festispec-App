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
        public ObservableCollection<AnswerVM> GetFilteredAnswersByString(string must_contain)
        {
            if (must_contain == null)
            {
                throw new ArgumentNullException(nameof(must_contain));
            }
            var _answers = CustomFSContext.Answers
                .ToList()
                .Where(c =>
                c.ToString().Contains(must_contain)
                ).Distinct()
                .Select(c => new AnswerVM(c));
            var _answer = new ObservableCollection<AnswerVM>(_answers);

            return _answer;

        }
        /*
         * Returns all AnswerVMs
         */

        public ObservableCollection<AnswerVM> GetAllAnswerVMs()
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
