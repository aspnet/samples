using ODataAuthorizationQueryValidatorSample.Extensions;

namespace ODataAuthorizationQueryValidatorSample.Model
{
    [CanExpand("Inspector")]
    public class Address
    {
        public int Id { get; set; }
    }
}
