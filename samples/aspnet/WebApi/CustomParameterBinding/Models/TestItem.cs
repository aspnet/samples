using System.Runtime.Serialization;

namespace CustomParameterBindingSample.Models
{
    public class TestItem
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}
