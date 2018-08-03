using System.Web.OData.Query;

namespace ODataQueryLimitationsSample.Model
{
    public class Address
    {
        [NonFilterable]
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public int ZipCode { get; set; }
    }
}
