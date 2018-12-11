# Domain Routing Policy Sample

This sample has a `DomainMatcherPolicy` for routing in ASP.NET Core 2.2 and above. The `DomainMatcherPolicy` extends ASP.NET routing to use the `host` header when matching a request to an MVC action. The `host` header value is matched with `[Domain]` attributes on controller and actions.

```cs
[Route("api/[controller]")]
[ApiController]
public class DomainController : ControllerBase
{
    [HttpGet]
    [Domain("contoso.com", "*.contoso.com")]
    public ActionResult<string> GetContoso()
    {
        return "Hello Contoso";
    }

    [HttpGet]
    [Domain("adventure-works.com", "*.adventure-works.com")]
    public ActionResult<string> GetAdventureWorks()
    {
        return "Hello AdventureWorks";
    }

    // Will be called as a fallback for other host values
    [HttpGet]
    public ActionResult<string> Get()
    {
        return "Hello World";
    }
}
```

## Features

* Match on the host name, port, or both
  * `contoso.com`
  * `*:80`
  * `contoso.com:80`
* Match with wildcards for subdomains, e.g. `*.contoso.com` will match `www.contoso.com`
* `DomainAttribute` can be placed on a controller or individual actions
* Controllers and actions with no `DomainAttribute` will match all `host` header values
* No match will return a 404 response.

## Configuration

Register the policy with dependency injection in your `Startup.cs`:

```cs
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, DomainMatcherPolicy.DomainMatcherPolicy>());
}
```

`DomainMatcherPolicy` requires ASP.NET Core 2.2 or above.