using System;

namespace ODataEnumTypeSample
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }

    [Flags]
    public enum AccessLevel
    {
        Read = 1,
        Write = 2,
        Execute = 4
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
