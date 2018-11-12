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
                .ForeignKey("dbo.Inspectors", t => t.InspectorId)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
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
                        CertificationDate = c.DateTime(nullable: false),
                        InvalidDate = c.DateTime(nullable: false),
                        BankNumber = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, storeType: "ntext"),
                        Comments = c.String(),
                        Options = c.String(),
                        Columns = c.String(),
                        QuestionnaireId = c.Int(nullable: false),
                        QuestionTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId)
                .Index(t => t.QuestionnaireId)
                .Index(t => t.QuestionTypeId);
            
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
                "dbo.Inspections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        State = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Adres = c.String(),
                        Place = c.String(),
                        ZipCode = c.String(),
                        StartingDate = c.DateTime(nullable: false),
                        ChamberOfCommerceNumber = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "QuestionTypeId", "dbo.QuestionTypes");
            DropForeignKey("dbo.Questions", "QuestionnaireId", "dbo.Questionnaires");
            DropForeignKey("dbo.Questionnaires", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "InspectorId", "dbo.Inspectors");
            DropForeignKey("dbo.Inspectors", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "RoleId", "dbo.Roles");
            DropIndex("dbo.Questionnaires", new[] { "InspectionId" });
            DropIndex("dbo.Questions", new[] { "QuestionTypeId" });
            DropIndex("dbo.Questions", new[] { "QuestionnaireId" });
            DropIndex("dbo.Inspectors", new[] { "AccountId" });
            DropIndex("dbo.Answers", new[] { "InspectorId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Accounts", new[] { "RoleId" });
            DropTable("dbo.Customers");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.Inspections");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.Questions");
            DropTable("dbo.Inspectors");
            DropTable("dbo.Answers");
            DropTable("dbo.Roles");
            DropTable("dbo.Accounts");
        }
    }
}
