using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ResponseEntityProcessorSample.Controllers
{
    /// <summary>
    /// This controller returns a simple collection of strings of varied size.
    /// </summary>
    public class ValuesController : ApiController
    {
        private static Random _random = new Random(0);
        private static object _thisLock = new object();

        public IEnumerable<string> Get()
        {
            int size = GetNext();
            List<string> result = new List<string>(size);
            for (int count = 0; count < size; count++)
            {
                result.Add(string.Format("String with value {0}", count));
            }

            return result;
        }

        private static int GetNext()
        {
            lock (_thisLock)
            {
                return _random.Next(0, 64 * 1024);
            };
        }
    }
}