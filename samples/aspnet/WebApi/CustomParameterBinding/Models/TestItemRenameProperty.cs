using System.Runtime.Serialization;

namespace CustomParameterBindingSample.Models
{
    public class TestItemRenameProperty
    {
        [DataMember(Name = "$Name")]
        public string Name { get; set; }
    }
}
