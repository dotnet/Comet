using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace HotUI.Blazor
{
    internal class CanvasWriter
    {
        private readonly IJSRuntime _jsRuntime;

        public CanvasWriter(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public Task<SizeF> GetSizeAsync(object canvas) => _jsRuntime.InvokeAsync<SizeF>("hotui.canvas.getSize", new[] { canvas });

        public Task DrawImageAsync(object canvas, byte[] bytes) => _jsRuntime.InvokeAsync<object>("hotui.canvas.drawImage", new[] { canvas, bytes });
    }
}
