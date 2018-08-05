using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleDemo
{
    public class TestController : ApiController
    {
        [HttpGet]
        public string Get(int id)
        {
            int result = 100 / id;
            return result.ToString();
        }
    }
}
