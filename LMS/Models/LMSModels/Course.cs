using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
        }

        public ushort CourseId { get; set; }
        public ushort Num { get; set; }
        public string Name { get; set; } = null!;
        public string Subject { get; set; } = null!;

        public virtual Department SubjectNavigation { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
    }
}
