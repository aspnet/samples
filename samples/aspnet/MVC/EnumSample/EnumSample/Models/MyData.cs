
namespace EnumSample.Models
{
    public class MyData
    {
        public int Id { get; set; }

        public MyEnum Enum1 { get; set; }

        public MyEnum? Enum2 { get; set; }

        public FlagsEnum FlagsEnum { get; set; }
    }
}