# Produces Routing Policy Sample

This sample has a `ProducesMatcherPolicy` that will match and select an endpoint using the request's `accept` header together with `[Produces]` on actions. The policy will select an endpoint instead of routing raising an error that multiple endpoints match the request. If no endpoint can match the `accept` header and there is no fallback endpoint then a [406 Not Acceptable](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/406) response is returned.

```cs
[Route("api/[controller]")]
[ApiController]
public class FallbackController : ControllerBase
{
    // Will be called for accept: application/json
    [HttpGet]
    [Produces("application/json")]
    public ActionResult<string> GetJson()
    {
        return "application/json";
    }

    // Will be called for accept: application/xml
    [HttpGet]
    [Produces("application/xml")]
    public ActionResult<string> GetXml()
    {
        return "application/xml";
    }

    // Will be called as a fallback
    [HttpGet]
    public ActionResult<string> Get()
    {
        return "*/*";
    }
}
```

Register the policy with dependency injection in your `Startup.cs`:

```cs
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ProducesMatcherPolicy.ProducesMatcherPolicy>());
}
```

`ProducesMatcherPolicy` requires ASP.NET Core 2.2 or above.