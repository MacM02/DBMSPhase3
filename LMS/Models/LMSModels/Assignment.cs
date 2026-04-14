using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignment
    {
        public Assignment()
        {
            Submissions = new HashSet<Submission>();
        }

        public string Name { get; set; } = null!;
        public string Contents { get; set; } = null!;
        public DateOnly? Due { get; set; }
        public uint Points { get; set; }
        public ushort AcId { get; set; }
        public ushort AssignmentId { get; set; }

        public virtual AssignmentCategory Ac { get; set; } = null!;
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
