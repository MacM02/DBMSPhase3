using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submission
    {
        public string StudentId { get; set; } = null!;
        public uint Score { get; set; }
        public string Contents { get; set; } = null!;
        public TimeOnly Time { get; set; }
        public ushort AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
