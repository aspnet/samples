using System.Runtime.Serialization;

namespace HostedClientCertificateSample.Models
{
    public class SampleItem
    {
        public int Id { get; set; }

        public string StringValue { get; set; }

        public override string ToString()
        {
            return Id + "," + StringValue;
        }
    }
}
