using DataAccess.WebApplication.Controllers;
using DataAccess.WebApplication.Data;
using DataAccess.WebApplication.Models;
using DeepEqual.Syntax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;

namespace DataAccess.Test
{
    public class StudentControllerTest
    {
        private StudentController _studentController;
        private StudentContext _studentContext;
        private IEnumerable<Student> _students;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<StudentContext>()
                .UseInMemoryDatabase("ExamTestDatabase")
                .Options;
            var studentContext = new StudentContext(options);
            _studentController = new StudentController(studentContext);

            _studentContext = new StudentContext(options);

            PopulateData();
        }

        [Test]
        public void IndexTest()
        {
            IActionResult indexResult = _studentController.Index();
            Assert.IsInstanceOf<ViewResult>(indexResult);
            var students = (indexResult as ViewResult).Model as List<Student>;
            students.ShouldDeepEqual(_students);
        }

        [TearDown]
        public void Teardown()
        {
            _studentContext.Database.EnsureDeleted();
        }

        private void PopulateData()
        {
            _students = new Student[]
            {
                new Student { Id = 1, Name = "Stu1", Subject = "Sub1" },
                new Student { Id = 2, Name = "Stu2", Subject = "Sub2" },
                new Student { Id = 3, Name = "Stu3", Subject = "Sub3" },
                new Student { Id = 4, Name = "Stu4", Subject = "Sub4" }
            };
            _studentContext.Students.AddRange(_students);
            _studentContext.SaveChanges();
        }
    }
}