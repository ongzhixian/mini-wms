using System.ComponentModel.DataAnnotations;

namespace Wms.Models.Data.Education
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int CourseID { get; set; }
        
        public int StudentID { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        //public Course Course { get; set; } = new Course();
        
        //public Student Student { get; set; } = new Student();
    }
}
