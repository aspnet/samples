using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttributeRoutingSample.Models
{
    public class Professor
    {
        public Professor()
        {
            this.Courses = new List<Course>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Course> Courses { get; set; }
    }
}