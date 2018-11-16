using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FSBeheer.VM;
using System.Threading.Tasks;
using FSBeheer.Crud;
using System.Data.Entity;

namespace FSBeheer
{
    class CustomFSContext: FSContext
    {
        public CustomerCrud CustomerCrud;
        public AnswerCrud AnswerCrud;
        public EventCrud EventCrud;
        public QuestionCrud QuestionCrud;
        public InspectionCrud InspectionCrud;
        public InspectorCrud InspectorCrud;

        public CustomFSContext() : base() {

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CustomFSContext, Migrations.Configuration>());

            CustomerCrud = new CustomerCrud(this);
            AnswerCrud = new AnswerCrud(this);
            EventCrud = new EventCrud(this);
            QuestionCrud = new QuestionCrud(this);
        }

        public ObservableCollection<InspectionVM> GetInspections()
        {
            var inspection = Inspections
                .ToList()
                .Select(i => new InspectionVM(i));
            var _inspections = new ObservableCollection<InspectionVM>(inspection);
            return _inspections;
        }

        public ObservableCollection<InspectorVM> GetInspectors()
        {
            var inspector = Inspectors
                .ToList()
                .Select(i => new InspectorVM(i));
            var _inspectors = new ObservableCollection<InspectorVM>(inspector);
            return _inspectors;
        }

    }
}
