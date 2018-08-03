using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace RouteConstraintsAndModelBindersSample.Server.ModelBinders
{
    /// <summary>
    /// Used to bind matrix parameter values from the URI.
    /// </summary>
    public class MatrixParameterModelBinder : IModelBinder
    {
        private readonly string _segment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixParameterModelBinder"/> class.
        /// </summary>
        /// <param name="segment">
        /// Can be empty, a target prefix value, or a route parameter name embeded in "{" and "}".
        /// </param>
        /// <example>
        /// If segment is null or empty, the parameter will match all color values from the whole path.
        /// If segment is "oranges", the parameter will match color only from the segment starting
        /// with "oranges" like .../oranges;color=red/...
        /// If segment is "{fruits}", the parameter will match color only from the route .../{fruits}/...
        /// </example>
        public MatrixParameterModelBinder(string segment)
        {
            _segment = segment;
        }

        public string Segment
        {
            get { return _segment; }
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            string attributeToBind = bindingContext.ModelName;

            // Match the route segment like [Route("{fruits}")] if possible.
            if (!String.IsNullOrEmpty(_segment)
                && _segment.StartsWith("{", StringComparison.Ordinal)
                && _segment.EndsWith("}", StringComparison.Ordinal))
            {
                string segmentName = _segment.Substring(1, _segment.Length - 2);
                ValueProviderResult segmentResult = bindingContext.ValueProvider.GetValue(segmentName);
                if (segmentResult == null)
                {
                    return false;
                }

                string matrixParamSegment = segmentResult.AttemptedValue;
                if (matrixParamSegment == null)
                {
                    return false;
                }

                IList<string> attributeValues = GetAttributeValues(matrixParamSegment, attributeToBind);
                if (attributeValues != null)
                {
                    bindingContext.Model = attributeValues.ToArray();
                }

                return true;
            }

            // Match values from segments like .../apples;color=red/..., then.
            ICollection<object> values = actionContext.ControllerContext.RouteData.Values.Values;

            // Expand in case that a catch-all constraint will deliver a segment with "/" in it.
            List<string> paramSegments = new List<string>();
            foreach (string segment in values)
            {
                paramSegments.AddRange(segment.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            }

            List<string> collectedAttributeValues = new List<string>();
            foreach (string paramSegment in paramSegments)
            {
                // If no parameter is specified, as [MatrixParam], get values from all the segments.
                // If a segment prefix is specified like [MatrixParam("apples")], get values only it is matched.
                if (!String.IsNullOrEmpty(_segment)
                    && !paramSegment.StartsWith(_segment + ";", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                IList<string> attributeValues = GetAttributeValues(paramSegment, attributeToBind);
                if (attributeValues != null)
                {
                    collectedAttributeValues.AddRange(attributeValues);
                }
            }

            bindingContext.Model = collectedAttributeValues.ToArray();
            return collectedAttributeValues.Count > 0;
        }

        private IList<string> GetAttributeValues(string matrixParamSegment, string attributeName)
        {
            NameValueCollection valuesCollection =
                HttpUtility.ParseQueryString(matrixParamSegment.Replace(";", "&"));
            string attributeValueList = valuesCollection.Get(attributeName);
            if (attributeValueList == null)
            {
                return null;
            }

            return attributeValueList.Split(',');
        }
    }
}
