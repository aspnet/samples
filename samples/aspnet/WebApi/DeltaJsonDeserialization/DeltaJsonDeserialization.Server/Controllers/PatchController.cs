using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.OData;

namespace DeltaJsonDeserialization.Server.Controllers
{
    [Route("api/Patch")]
    public class PatchController : ApiController
    {
        public IHttpActionResult Patch(Delta<SampleModel> delta)
        {
            // Using the Patch method on Delta<T>, will only overwrite only the properties whose value has
            // changed.
            var model = new SampleModel();
            delta.Patch(model);

            // Using Delta doesn't invoke validation on the values that are provided, so use the Validate method
            // on the model object after patching to validate it.
            this.Validate(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var builder = new StringBuilder();
            builder.AppendLine("Updated Properties:");

            foreach(var property in delta.GetChangedPropertyNames())
            {
                object value;
                delta.TryGetPropertyValue(property, out value);

                builder.AppendLine(String.Format("\t{0} : {1}", property, value));
            }

            return Text(builder.ToString());
        }

        private IHttpActionResult Text(string text)
        {
            return new PlainTextHttpActionResult(Request, text);
        }
    }
}