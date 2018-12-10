using System.IO;
using DomainMatcherPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;

namespace MvcSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly DfaGraphWriter _graphWriter;
        private readonly CompositeEndpointDataSource _compositeEndpointDataSource;

        public GraphController(DfaGraphWriter graphWriter, CompositeEndpointDataSource compositeEndpointDataSource)
        {
            _graphWriter = graphWriter;
            _compositeEndpointDataSource = compositeEndpointDataSource;
        }

        public ActionResult<string> Get()
        {
            StringWriter sw = new StringWriter();
            _graphWriter.Write(_compositeEndpointDataSource, sw);

            return sw.ToString();
        }
    }
}