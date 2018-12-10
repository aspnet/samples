# Domain Routing Policy Sample

This sample has a `DomainMatcherPolicy` for routing in ASP.NET Core 2.2 and above. This policy will match an endpoint using the request's `host` header together with `[Domain]` on actions. If no endpoint matches the `host` header, and there is no fallback endpoint, then a 404 response is returned.

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

Register the policy with dependency injection in your `Startup.cs`:

```cs
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, DomainMatcherPolicy.DomainMatcherPolicy>());
}
```

`DomainMatcherPolicy` requires ASP.NET Core 2.2 or above.