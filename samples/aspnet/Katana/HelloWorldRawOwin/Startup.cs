using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace HelloWorld
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    // Note the Web.Config owin:AutomaticAppStartup setting that is used to direct all requests to your OWIN application.
    // Alternatively you can specify routes in the global.asax file.
    public class Startup
    {
        // Invoked once at startup to configure your application.
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(new Func<AppFunc, AppFunc>(ignoredNextApp => (AppFunc)Invoke));
        }

        // Invoked once per request.
        public Task Invoke(IDictionary<string, object> environment)
        {
            string responseText = "Hello World";
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);

            // See http://owin.org/spec/owin-1.0.0.html for standard environment keys.
            Stream responseStream = (Stream)environment["owin.ResponseBody"];
            IDictionary<string, string[]> responseHeaders =
                (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];

            responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
            responseHeaders["Content-Type"] = new string[] { "text/plain" };

            return Task.Factory.FromAsync(responseStream.BeginWrite, responseStream.EndWrite, responseBytes, 0, responseBytes.Length, null);
            // 4.5: return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}