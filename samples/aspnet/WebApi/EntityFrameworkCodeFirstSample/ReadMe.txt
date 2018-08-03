EntityFrameworkCodeFirstSample
------------------------------

This sample illustrates how to build a Web API which is backed by Entity
Framework using Code First to create the database.

The functionality is exposed in the AttractionsController which, given a set
of spatial coordinates, provides information about nearby attractions.

NOTE: After compiling the sample FOR THE FIRST TIME please CLOSE AND 
RE-OPEN the solution to work around a NuGet package restore bug.

Before running the sample you need to seed the database with some data. To
do this run the following command from the "Package Manager Console" which is
available under the Tools/Library Package Manager menu in Visual Studio:

  Update-Database

The process for seeding the database is described in detail in the blog below.

If you get an error looking like this then please close and re-open the solution
and run the command again:

    Update-Database : The term 'Update-Database' is not recognized as the name
    of a cmdlet, function, script file, or operable program. Check the spelling 
    of the name, or if a path was included, verify that the path is correct 
    and try again.

When running the sample, try typing the following coordinates into the HTML form:

  Longitude: -122.35
  Latitude: 47.61

For a detailed description of this sample, please see
http://blogs.msdn.com/b/jasonz/archive/2012/07/23/my-favorite-features-entity-framework-code-first-and-asp-net-web-api.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487