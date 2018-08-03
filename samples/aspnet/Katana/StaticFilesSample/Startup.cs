using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace StaticFilesSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            // Remap '/' to '.\defaults\'.
            // Turns on static files and default files.
            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(@".\defaults"),
            });

            // Only serve files requested by name.
            app.UseStaticFiles("/files");

            // Turns on static files, directory browsing, and default files.
            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = new PathString("/public"),
                EnableDirectoryBrowsing = true,
            });

            // Browse the root of your application (but do not serve the files).
            // NOTE: Avoid serving static files from the root of your application or bin folder,
            // it allows people to download your application binaries, config files, etc..
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                RequestPath = new PathString("/src"),
                FileSystem = new PhysicalFileSystem(@""),
            });

            // Anything not handled will land at the welcome page.
            app.UseWelcomePage();
        }
    }
}
