using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttributeRoutingSample.Models
{
    public class Course
    {
        public Course()
        {
            this.Students = new List<Student>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Professor Professor { get; set; }

        public List<Student> Students { get; set; }
    }
}