using System;
using System.ComponentModel.DataAnnotations;

namespace hw2.ExModels
{
    public partial class CourseDepartmentPerson
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public DateTime DepartmentDateModified { get; set; }
        public string Name { get; set; }
        public DateTime? CourseDateModified { get; set; }
        public int? InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}