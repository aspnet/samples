Enums in MVC sample
-------------------

This ASP.NET MVC sample illustrates how to use the extended HtmlHelper (the
class behind @Html) support for enum datatypes and the new EnumHelper class (a
few static helpers for creating select lists for enum datatypes).

For the most part, the project is unchanged from the default ASP.NET MVC 5.0
project with scaffolding of a simple model to generate an Entity Framework
controller. The primary changes of interest are found in the
Views\Shared\DisplayTemplates and ..\EditorTemplates folders. The files there
provide custom templates for DisplayFor() and EditorFor(), overriding the
default templates for enum expressions. In addition Views\Home\Edit.cshtml
names specific templates (passes the EditorFor() templateName parameter) to use
the Views\Shared\EditorTemplates\Enum-radio.cshtml template for two of the enum
properties.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487