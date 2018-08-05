using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace RouteConstraintsAndModelBindersSample.Server.ModelBinders
{
    /// <summary>
    /// Used to bind matrix parameter values from the URI.
    /// </summary>
    public class MatrixParameterAttribute : ModelBinderAttribute
    {
        private readonly string _segment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixParameterAttribute"/> class.
        /// </summary>
        /// <example>
        /// <c>[MatrixParam] string[] color</c> will match all color values from the whole path.
        /// </example>
        public MatrixParameterAttribute()
            : this(segment: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixParameterAttribute"/> class.
        /// </summary>
        /// <param name="segment">
        /// Can be empty, a target prefix value, or a general route name embeded in "{" and "}".
        /// </param>
        /// <example>
        /// <c>[MatrixParam("")] string[] color</c> will match all color values from the whole path.
        /// <c>[MatrixParam("oranges")] string[] color</c> will match color only from the segment starting
        /// with "oranges" like .../oranges;color=red/...
        /// <c>[MatrixParam("{fruits}")] string[] color</c> will match color only from the route .../{fruits}/...
        /// </example>
        public MatrixParameterAttribute(string segment)
        {
            _segment = segment;
        }

        public string Segment
        {
            get { return _segment; }
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            HttpConfiguration config = parameter.Configuration;
            IEnumerable<ValueProviderFactory> valueProviderFactories = GetValueProviderFactories(config);

            return new ModelBinderParameterBinding(parameter,
                                                   new MatrixParameterModelBinder(_segment),
                                                   valueProviderFactories);
        }
    }
}
