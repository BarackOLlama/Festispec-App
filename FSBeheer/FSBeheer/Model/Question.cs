namespace FSBeheer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }

        public int QuestionTypeId { get; set; }

        public int QuestionnaireId { get; set; }

        public string Options { get; set; }

        public string Columns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual QuestionType QuestionType { get; set; }
    }
}
