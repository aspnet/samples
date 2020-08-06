using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MjrChess.Trainer.BlazorExtensions
{
    /// <summary>
    /// Component base class that invokes JavaScript to register material design
    /// elements in the component in OnAfterRenderAsync.
    /// </summary>
    public class MaterialDesignComponentBase

        // OwningComponentBase helps with service lifetime issues
        // https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-3.1#utility-base-component-classes-to-manage-a-di-scope
        : OwningComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!; // Injected service, so no initialization needed

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // MDC components need JavaScript initialization,
                // so make sure to invoke attachMDC once the MDC elements are rendered.
                await AttachMDCAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task AttachMDCAsync()
        {
            // Call JS helper to attach MDC elements
            await JSRuntime.InvokeVoidAsync("attachMDC");
        }
    }
}
