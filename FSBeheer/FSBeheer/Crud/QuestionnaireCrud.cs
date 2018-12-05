﻿using FSBeheer.ViewModel;
using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class QuestionnaireCrud : AbstractCrud
    {

        public QuestionnaireCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<QuestionnaireVM> GetAllQuestionnaires()
        {
            var questionnaire = CustomFSContext.Questionnaires
                .ToList()
                .Select(i => new QuestionnaireVM(i));
            var _questionnaire = new ObservableCollection<QuestionnaireVM>(questionnaire);
            return _questionnaire;
        }

        public void Add(QuestionnaireVM _questionnaire)
        {
            CustomFSContext.Questionnaires.Add(_questionnaire.ToModel());
            CustomFSContext.SaveChanges();
        }

        public ObservableCollection<QuestionnaireVM> GetAllQuestionnairesFiltered(string must_contain)
        {
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var questionnaires = CustomFSContext.Questionnaires
                .ToList()
                .Where(c => c.IsDeleted == false)
                .Where(e =>
                e.Id.ToString().ToLower().Contains(must_contain) ||
                e.Name.ToLower().Contains(must_contain) ||
                e.Version.ToString().ToLower().Contains(must_contain)
                ).Distinct()
                .Select(e => new QuestionnaireVM(e));
            return new ObservableCollection<QuestionnaireVM>(questionnaires);
        }
    }
}
