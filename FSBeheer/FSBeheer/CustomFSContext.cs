﻿using System;
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
    class CustomFSContext : FSContext
    {
        public CustomerCrud CustomerCrud;
        public AnswerCrud AnswerCrud;
        public EventCrud EventCrud;
        public QuestionCrud QuestionCrud;
        public InspectionCrud InspectionCrud;
        public InspectorCrud InspectorCrud;
        public AvailabilityCrud AvailabilityCrud;
        public InspectionDateCrud InspectionDateCrud;
        public QuestionnaireCrud QuestionnaireCrud;

        public CustomFSContext() : base()
        {

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CustomFSContext, Migrations.Configuration>());

            CustomerCrud = new CustomerCrud(this);
            AnswerCrud = new AnswerCrud(this);
            EventCrud = new EventCrud(this);
            QuestionCrud = new QuestionCrud(this);
            InspectionCrud = new InspectionCrud(this);
            InspectorCrud = new InspectorCrud(this);
            AvailabilityCrud = new AvailabilityCrud(this);
            InspectionDateCrud = new InspectionDateCrud(this);
            QuestionnaireCrud = new QuestionnaireCrud(this);
        }
    }
}
