using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Class
    {
        public Class()
        {
            AssignmentCategories = new HashSet<AssignmentCategory>();
            Enrolleds = new HashSet<Enrolled>();
        }

        public string Loc { get; set; } = null!;
        public ushort Year { get; set; }
        public string Season { get; set; } = null!;
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public string PuId { get; set; } = null!;
        public ushort CourseId { get; set; }
        public ushort ClassId { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Professor Pu { get; set; } = null!;
        public virtual ICollection<AssignmentCategory> AssignmentCategories { get; set; }
        public virtual ICollection<Enrolled> Enrolleds { get; set; }
    }
}
