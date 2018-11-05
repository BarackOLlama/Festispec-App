namespace FSBeheer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSBeheer.Model;

    public partial class FSContext : DbContext
    {
        public FSContext()
            : base("name=FSDB")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<FSContext>());
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Account> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
