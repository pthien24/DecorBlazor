using Microsoft.JSInterop;

namespace DecorWeb_Server.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime jSRuntime ,string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }

        public static async ValueTask ToastrError(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }

        public static async ValueTask Sweet2Success(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowSweet2", "success", message);
        }

        public static async ValueTask Sweet2Error(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowSweet2", "error", message);
        }
    }
}
