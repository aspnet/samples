using DataAccess.WebApplication.Controllers;
using DataAccess.WebApplication.Data;
using DataAccess.WebApplication.Models;
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
            Assert.IsNotNull(students);

            Assert.AreEqual(1, students[0].Id);
            Assert.AreEqual("Stu1", students[0].Name);
            Assert.AreEqual("Sub1", students[0].Subject);

            Assert.AreEqual(2, students[1].Id);
            Assert.AreEqual("Stu2", students[1].Name);
            Assert.AreEqual("Sub2", students[1].Subject);

            Assert.AreEqual(3, students[2].Id);
            Assert.AreEqual("Stu3", students[2].Name);
            Assert.AreEqual("Sub3", students[2].Subject);

            Assert.AreEqual(4, students[3].Id);
            Assert.AreEqual("Stu4", students[3].Name);
            Assert.AreEqual("Sub4", students[3].Subject);
        }

        [TearDown]
        public void Teardown()
        {
            var students = _studentContext.Students;
            _studentContext.Students.RemoveRange(students);
            _studentContext.SaveChanges();
        }

        private void PopulateData()
        {
            _studentContext.Students.AddRange(
                new Student { Id = 1, Name = "Stu1", Subject = "Sub1" },
                new Student { Id = 2, Name = "Stu2", Subject = "Sub2" },
                new Student { Id = 3, Name = "Stu3", Subject = "Sub3" },
                new Student { Id = 4, Name = "Stu4", Subject = "Sub4" });
            _studentContext.SaveChanges();
        }
    }
}