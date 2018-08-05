using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AttributeRoutingSample.Models
{
    public class Student
    {
        public Student()
        {
            this.Courses = new List<Course>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public List<Course> Courses { get; set; }
    }
}        