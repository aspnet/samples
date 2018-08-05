using System.Web.Http;
using JsonUploadSample.Models;

namespace JsonUploadSample.Controllers
{
    public class ContactController : ApiController
    {
        public Contact Post(Contact contact)
        {
            return contact;
        }
    }
}
