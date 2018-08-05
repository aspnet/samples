using System.Text;
using System.Web.Http;
using ActionResults.Results;

namespace ActionResults.Controllers
{
    public class HomeController : ApiController
    {
        [Route("text")]
        public OkTextPlainResult GetText()
        {
            return this.Text("Hello, world!");
        }

        [Route("text_ascii")]
        public OkTextPlainResult GetTextAscii()
        {
            return this.Text("Hello, world!", Encoding.ASCII);
        }

        [Route("file")]
        public OkFileDownloadResult GetFile()
        {
            return this.Download("Download.xml", "application/xml");
        }
    }
}