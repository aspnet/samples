# Produces Routing Policy Sample

This sample has a `ProducesMatcherPolicy` that will match and select an endpoint using the request's `accept` header together with `[Produces]` on actions.

* Requests to `FallbackController` that do not match an action's `[Produces]` media type will fallback to the action without an attribute
* Requests to `StrictController` that do not match an action's `[Produces]` media type will return 404

Register the policy with dependency injection in your `Startup.cs`:

```cs
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ProducesMatcherPolicy.ProducesMatcherPolicy>());
}
```

`ProducesMatcherPolicy` requires ASP.NET Core 2.2 or above.