using ODataAuthorizationQueryValidatorSample.Extensions;

namespace ODataAuthorizationQueryValidatorSample.Model
{
    [CanExpand("Manager")]
    public class OrderLine
    {
        public int Id { get; set; }
    }
}
