using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AttributeRoutingSample.Models
{
    public class SchoolContextInitializer : DropCreateDatabaseAlways<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            Student rob = new Student();
            rob.FirstName = "Rob";
            rob.LastName = "Hindman";

            Student mike = new Student();
            mike.FirstName = "Mike";
            mike.LastName = "Stall";

            Student dan = new Student();
            dan.FirstName = "Dan";
            dan.LastName = "Randall";

            context.Students.Add(rob);
            context.Students.Add(mike);
            context.Students.Add(dan);
            context.SaveChanges();

            Course algorithms = new Course();
            algorithms.Name = "Algorithms";
            algorithms.Students.Add(rob);
            algorithms.Students.Add(mike);

            Course networking = new Course();
            networking.Name = "Computer Networks";
            networking.Students.Add(dan);
            networking.Students.Add(rob);

            Course cryptography = new Course();
            cryptography.Name = "Cryptography";
            cryptography.Students.Add(dan);
            cryptography.Students.Add(rob);
            cryptography.Students.Add(mike);

            context.Courses.Add(algorithms);
            context.Courses.Add(networking);
            context.Courses.Add(cryptography);
            context.SaveChanges();

            Professor james = new Professor();
            james.Name = "James King";
            james.Courses.Add(networking);

            Professor richard = new Professor();
            richard.Name = "Richard Jenkins";
            richard.Courses.Add(cryptography);

            Professor steve = new Professor();
            steve.Name = "Steve Cutler";
            steve.Courses.Add(algorithms);

            context.Professors.Add(james);
            context.Professors.Add(richard);
            context.Professors.Add(steve);
            context.SaveChanges();
        }
    }
}
        