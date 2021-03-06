﻿using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Core.Data
{
    public class Student : BaseEntity
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
