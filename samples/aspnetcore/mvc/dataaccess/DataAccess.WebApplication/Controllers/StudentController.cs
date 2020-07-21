using DataAccess.WebApplication.Data;
using DataAccess.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.WebApplication.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentContext _studentContext;

        public StudentController(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }

        public IActionResult Index()
        {
            List<Student> students = _studentContext.Students
                .OrderBy(student => student.Id)
                .ToList();
            return View(students);
        }
    }
}
