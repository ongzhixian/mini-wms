﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Wms.Models.Data.Education
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public int Credits { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
