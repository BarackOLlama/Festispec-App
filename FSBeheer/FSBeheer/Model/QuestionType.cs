using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FSBeheer.Model
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Questions = new ObservableCollection<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ObservableCollection<Question> Questions { get; set; }
    }
}
