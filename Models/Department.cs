using System;
using System.Collections.Generic;

namespace hw2.Models
{
    public partial class Department
    {
        public Department()
        {
            Course = new HashSet<Course>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int? InstructorId { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime DateModified { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Person Instructor { get; set; }
        public virtual ICollection<Course> Course { get; set; }
    }
}
