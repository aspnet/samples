using DataAccess.WebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.WebApplication.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
