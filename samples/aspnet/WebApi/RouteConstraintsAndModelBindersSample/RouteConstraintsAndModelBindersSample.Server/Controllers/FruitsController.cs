using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;
using RouteConstraintsAndModelBindersSample.Server.ModelBinders;

namespace RouteConstraintsAndModelBindersSample.Server.Controllers
{
    [RoutePrefix("customers/{customerId}")]
    public class FruitsController : ApiController
    {
        // GET customers/2/bananas;color=yellow,green;rate=good/oregon
        //
        // This route with the segments {fruits} and {location} will match a path with two segments if they are not 
        // matched with the following two actions GetApplesFromWashington and GetApplesFromLocation. Both of their 
        // routes are more specific because of constraints, and thus matched prior to this.
        [HttpGet]
        [Route("{fruits}/{location}")] 
        public IHttpActionResult GetFruitsFromLocation([SegmentPrefix] string fruits, // The fruits from the route segment {fruits}.
                                                       [MatrixParameter("bananas")] string[] color, // The matrix parameter color from the segment starting with "bananas". It is matched only if the fruits is "apples".
                                                       [SegmentPrefix] string location, // The location from the route segment {location}.
                                                       [MatrixParameter("{fruits}")] string[] rate) // The matrix parameter rate from the route segment "{fruits}".
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                {"fruits", fruits},
                {"color of bananas", Join(color)},
                {"location", location},
                {"rate of " + fruits, Join(rate)},
            };
            return Json(result);

        }

        // GET customers/1/apples;rate=excellent;color=red/washington
        //
        // This route with segment prefixes "apples" and then "washington" (both defined by SegmentPrefixConstraint 
        // will match a path with two segments in which the first starts with "apples" and the second starts with 
        // "washington", like "/apples;.../washington;...".
        [HttpGet]
        [Route("{apples:SegmentPrefix}/{washington:SegmentPrefix}")]
        public IHttpActionResult GetApplesFromWashington(long customerId, // The customer's id from the route segment {customerId} of the URI.
                                                         [MatrixParameter] string[] color, // The matrix parameter color from any path segment of the URI.
                                                         [MatrixParameter("apples")] string[] rate) //The matrix parameter rate from the path segment starting with "apples" of the URI.
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                {"customerId", customerId.ToString(CultureInfo.InvariantCulture)},
                {"color", Join(color)},
                {"rate of apples", Join(rate)}
            };
            return Json(result);
        }

        // GET customers/2/apples;color=red;rate=good;color=green;/connecticut;rate=excellent
        //
        // This route with a segment prefix "apples" (defined by SegmentPrefixConstraint) and then the route
        // segment {location} will match a path with two segments, in which the first starts with "apples".
        [HttpGet]
        [Route("{apples:SegmentPrefix}/{location}")]
        public IHttpActionResult GetApplesFromLocation(long customerId, // The customer's id from the route segment {customerId}.
                                                       [SegmentPrefix] string location, // The segment prefix location from the route segment {location}.
                                                       [MatrixParameter] string[] color, // The matrix parameter color from any path segment of the URI.
                                                       [MatrixParameter("{location}")] string[] rate) //The matrix parameter rate from the route segment {location}.
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                {"customerId", customerId.ToString(CultureInfo.InvariantCulture)},
                {"location", location},
                {"color", Join(color)},
                {"rate of " + location, Join(rate)}
            };
            return Json(result);
        }

        // GET customers/2/optional/foo/apples;rate=good/california;/bar/color=green,red;rate=excellent,good
        //
        // This route with the catch-all constraint {*OptionalSubPath} will match all segments following "optional/".
        [HttpGet]
        [Route("optional/{*OptionalSubPath}")]
        public IHttpActionResult GetAttributesFromOptionalSegments([MatrixParameter] string[] color, // The color from any path segment of the URI.
                                                                   [MatrixParameter("apples")] string[] rate) // The rate from a path segment starting with the prefix "apples".
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                {"color", Join(color)},
                {"rate of apples", Join(rate)}
            };
            return Json(result);
        }

        private string Join(string[] array)
        {
            return array == null ? String.Empty : String.Join(",", array);
        }
    }
}
