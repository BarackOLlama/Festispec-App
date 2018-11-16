namespace FSBeheer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSBeheer.Model;

    public partial class FSContext : DbContext
    {
        internal object temp;

        public FSContext()
            : base("name=FSDB")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<FSContext, Migrations.Configuration>());
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Inspection> Inspections { get; set; }
        public virtual DbSet<Inspector> Inspectors { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
