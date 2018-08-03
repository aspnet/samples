MemoryTracingSample
-------------------

This sample project creates a Nuget package that will install a custom
in-memory trace writer into ASP.NET MVC Web API applications.  

This in-memory trace writer provides a visual history of the Web API framework 
operations performed for each HTTP request and can be used to debug and
diagnose running applications.

This sample illustrates:
    - How to create a custom ITraceWriter to observe WebApi traces.
	- How to create a Nuget package that will install this custom writer
	  into another Web API application.


Building the Nuget package
--------------------------
Building this sample solution builds a Nuget package.  Look for the .nupkg
file under the bin/Debug folder.

That Nuget package will contain all the source files from this project,
ready to be installed into any other target Web API project.


Installing the Nuget package
----------------------------
Once you have built this Nuget package, you can install it into an
ASP.NET MVC Web API application.  Installing this Nuget package adds
source code for the in-memory trace writer to that target application. 

To install the Nuget package built by this sample:

  (1) Create or open an ASP.NET MVC Web API application where
      you would like to install the in-memory trace writer.
      (File | New | Project | Web | ASP.NET Web Application | Web API)

  (2) Open the Nuget package manager UI in Visual Studio
      (Right-click project | Manage Nuget Packages)

  (3) [One time only, do these steps]
      (3a) Click the Settings button in the package manager UI

      (3b) Add a new package source that points to this
           sample's bin/Debug folder so that it can find
		   the generated .nupkg file. OK to close Settings.

  (4) You should now see your new package source in the
      Package Manager UI.  Click on the package called
	  Microsoft.AspNet.WebApi.MemoryTracing.Sample and
	  click Install to install it into the target app.


What the Nuget package installs
-------------------------------
When you install the Nuget package built by this sample into another
application, it simply inserts source code into that application.  
There is no binary associated with this package.

The source code you see under the Areas\Trace folder of this sample project
is written to the Areas\Trace folder of the target project and takes advantage
of MVC Areas to self-register at app start-up.

You are free to modify the code installed by the Nuget package into the
target application.  Any changes you make will not be deleted when you
uninstall the package.


Using the MemoryTracing package
-------------------------------
Once you have installed the Nuget package into an MVC Web API application,
you can run that application.  No other setup is required.

The installed code will automatically register itself and capture to memory
all traces written by the Web API framework code in response to incoming
HTTP requests.

To see these traces browse to the application's root URL + "/Trace"
(example: http://localhost:40837/Trace).

You will see an MVC page showing the requests what have been received.
Clicking on the "Traces..." link for any request will display all the
traces written by the Web API framework when handling that request.

You can use this information to debug your application.  You can also
deploy your application and monitor its trace history remotely.  But
if you do, be sure to secure the TraceController against access by
unauthorized users.

Disabling other tracers
-----------------------
If the URL+"/Trace" view does not show any traces, it is likely you
have another tracer registered.   It must be disabled for this
memory trace writer to work.

For example, the standard MVC application has this line in Area_Start\WebApi.Config:

      config.EnableSystemDiagnosticsTracing();

You must comment out that line after installing the memory trace writer.

Diabling memory tracing
------------------------
If you wish to disable the in-memory trace writer but don't want
to uninstall the Nuget package, simply comment out the code in
TraceAreaRegistration.RegisterArea method.

Uninstalling the Nuget package
------------------------------
Use the same Package Manager UI or the package manager console
to uninstall Microsoft.AspNet.WebApi.MemoryTracing.Sample.  This
will remove the Areas\Trace folder from the project.  Any files
you have modified under Areas\Trace will not be removed.

   ***WARNING*** You will be asked whether you want to uninstall
                 the packages this sample package depends on.
				 Answer "No" or you they will be removed from
				 your project and require re-installation.


How this sample project works
-----------------------------
When you build this sample, custom MSBuild targets prepare a Nuget package from
the included .nuspec file.  The source code in this project is compiled for 
correctness and then written into the Nuget package as source code.

This sample is not the In-Memory trace writer executable itself.  Rather, it is
a Nuget package builder.  When the built Nuget package is installed into another
application, it will create the in-memory trace writer in that app.


How the installed in-memory trace writer works
----------------------------------------------
The in-memory trace writer installed by the Nuget package is written as source
code directly into the target application's Areas\Trace folder.  When that app
is compiled, it builds the in-memory trace writer code within that app.

When the app is started, the built-in MVC 4 support for "Areas" locates the
TraceAreaRegistration class and calls it to give it a chance to register
itself.  This class creates a route to the TraceController and invokes
TraceConfig to register the custom trace writer.  TraceConfig creates
the in-memory trace writer and registers it with the HttpConfiguration.

Whenever an HTTP request is received, the Web API framework writes traces
to any ITraceWriter found in the HttpConfiguration.  The in-memory trace
writer captures these traces and makes them available to the MVC TraceController.

The memory buffer used by the trace writer is a simple rolling buffer and
will discard the oldest traces when its buffer is full.  It does not write
the traces to a persistent store.  You can change the size of the memory
buffer by changing the default size used in the MemoryTraceStore constructor.

For more information about debugging Web API applications refer to
http://www.asp.net/web-api

For more information about ITraceWriter and tracing in Web API refer to
http://blogs.msdn.com/b/roncain/archive/2012/04/12/tracing-in-asp-net-web-api.aspx


This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
