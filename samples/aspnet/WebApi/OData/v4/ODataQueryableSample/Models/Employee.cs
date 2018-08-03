using System.Collections.Generic;

namespace ODataQueryableSample.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Employee Friend { get; set; }
        public Manager Manager { get; set; }
    }

    public class Manager : Employee
    {
        public List<Employee> DirectReports { get; set; } 
    }
}
