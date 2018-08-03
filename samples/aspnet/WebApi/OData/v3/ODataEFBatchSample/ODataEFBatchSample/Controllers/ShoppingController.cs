using System.Web.Http.OData;
using ODataEFBatchSample.Extensions;
using ODataEFBatchSample.Models;

namespace ODataEFBatchSample.Controllers
{
    public class ShoppingController : ODataController
    {
        private ShoppingContext _context;

        public ShoppingContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = this.Request.GetContext();
                }

                return _context;
            }
        }
    }
}