using System;
using System.Collections.Generic;

namespace hw2.Models
{
    public partial class Person
    {
        public Person()
        {
            CourseInstructor = new HashSet<CourseInstructor>();
            Department = new HashSet<Department>();
            Enrollment = new HashSet<Enrollment>();
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string Discriminator { get; set; }
        public DateTime DateModified { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual OfficeAssignment OfficeAssignment { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructor { get; set; }
        public virtual ICollection<Department> Department { get; set; }
        public virtual ICollection<Enrollment> Enrollment { get; set; }
    }
}
