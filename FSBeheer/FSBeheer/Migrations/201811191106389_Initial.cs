namespace FSBeheer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        RoleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        QuestionId = c.Int(),
                        InspectorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .ForeignKey("dbo.Inspectors", t => t.InspectorId)
                .Index(t => t.QuestionId)
                .Index(t => t.InspectorId);
            
            CreateTable(
                "dbo.Inspectors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Zipcode = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        CertificationDate = c.DateTime(),
                        InvalidDate = c.DateTime(),
                        BankNumber = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Inspections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Notes = c.String(),
                        EventId = c.Int(),
                        StatusId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.EventId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Adres = c.String(),
                        City = c.String(),
                        ZipCode = c.String(),
                        StartingDate = c.DateTime(),
                        ChamberOfCommerceNumber = c.Short(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Note = c.String(),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.EventDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        StartTime = c.Time(precision: 7),
                        EndTime = c.Time(precision: 7),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.InspectionDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        StartTime = c.Time(precision: 7),
                        EndTime = c.Time(precision: 7),
                        Inspection_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.Inspection_Id)
                .Index(t => t.Inspection_Id);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Instructions = c.String(),
                        Version = c.Int(nullable: false),
                        Comments = c.String(),
                        InspectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.InspectionId)
                .Index(t => t.InspectionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Comments = c.String(),
                        Options = c.String(),
                        Columns = c.String(),
                        QuestionnaireId = c.Int(),
                        QuestionTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireId)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId)
                .Index(t => t.QuestionnaireId)
                .Index(t => t.QuestionTypeId);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Availabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Scheduled = c.Boolean(nullable: false),
                        AvailableStartTime = c.Time(precision: 7),
                        AvailableEndTime = c.Time(precision: 7),
                        ScheduleStartTime = c.Time(precision: 7),
                        ScheduleEndTime = c.Time(precision: 7),
                        InspectorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspectors", t => t.InspectorId)
                .Index(t => t.InspectorId);
            
            CreateTable(
                "dbo.Quotations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(precision: 10, scale: 2),
                        Date = c.DateTime(),
                        Description = c.String(),
                        CustomerId = c.Int(),
                        InspectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Inspections", t => t.InspectionId)
                .Index(t => t.CustomerId)
                .Index(t => t.InspectionId);
            
            CreateTable(
                "dbo.InspectionInspectors",
                c => new
                    {
                        Inspection_Id = c.Int(nullable: false),
                        Inspector_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Inspection_Id, t.Inspector_Id })
                .ForeignKey("dbo.Inspections", t => t.Inspection_Id, cascadeDelete: true)
                .ForeignKey("dbo.Inspectors", t => t.Inspector_Id, cascadeDelete: true)
                .Index(t => t.Inspection_Id)
                .Index(t => t.Inspector_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotations", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Quotations", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Availabilities", "InspectorId", "dbo.Inspectors");
            DropForeignKey("dbo.Answers", "InspectorId", "dbo.Inspectors");
            DropForeignKey("dbo.Inspections", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Questions", "QuestionTypeId", "dbo.QuestionTypes");
            DropForeignKey("dbo.Questions", "QuestionnaireId", "dbo.Questionnaires");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questionnaires", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.InspectionInspectors", "Inspector_Id", "dbo.Inspectors");
            DropForeignKey("dbo.InspectionInspectors", "Inspection_Id", "dbo.Inspections");
            DropForeignKey("dbo.InspectionDates", "Inspection_Id", "dbo.Inspections");
            DropForeignKey("dbo.Inspections", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventDates", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Contacts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Inspectors", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "RoleId", "dbo.Roles");
            DropIndex("dbo.InspectionInspectors", new[] { "Inspector_Id" });
            DropIndex("dbo.InspectionInspectors", new[] { "Inspection_Id" });
            DropIndex("dbo.Quotations", new[] { "InspectionId" });
            DropIndex("dbo.Quotations", new[] { "CustomerId" });
            DropIndex("dbo.Availabilities", new[] { "InspectorId" });
            DropIndex("dbo.Questions", new[] { "QuestionTypeId" });
            DropIndex("dbo.Questions", new[] { "QuestionnaireId" });
            DropIndex("dbo.Questionnaires", new[] { "InspectionId" });
            DropIndex("dbo.InspectionDates", new[] { "Inspection_Id" });
            DropIndex("dbo.EventDates", new[] { "Event_Id" });
            DropIndex("dbo.Contacts", new[] { "CustomerId" });
            DropIndex("dbo.Events", new[] { "CustomerId" });
            DropIndex("dbo.Inspections", new[] { "StatusId" });
            DropIndex("dbo.Inspections", new[] { "EventId" });
            DropIndex("dbo.Inspectors", new[] { "AccountId" });
            DropIndex("dbo.Answers", new[] { "InspectorId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Accounts", new[] { "RoleId" });
            DropTable("dbo.InspectionInspectors");
            DropTable("dbo.Quotations");
            DropTable("dbo.Availabilities");
            DropTable("dbo.Status");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.Questions");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.InspectionDates");
            DropTable("dbo.EventDates");
            DropTable("dbo.Contacts");
            DropTable("dbo.Customers");
            DropTable("dbo.Events");
            DropTable("dbo.Inspections");
            DropTable("dbo.Inspectors");
            DropTable("dbo.Answers");
            DropTable("dbo.Roles");
            DropTable("dbo.Accounts");
        }
    }
}
