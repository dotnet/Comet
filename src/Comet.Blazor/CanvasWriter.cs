using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Comet.Blazor
{
    internal class CanvasWriter
    {
        private readonly IJSRuntime _jsRuntime;

        public CanvasWriter(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public Task<SizeF> GetSizeAsync(object canvas) => _jsRuntime.InvokeAsync<SizeF>("comet.canvas.getSize", new[] { canvas });

        public Task DrawImageAsync(object canvas, byte[] bytes) => _jsRuntime.InvokeAsync<object>("comet.canvas.drawImage", new[] { canvas, bytes });
    }
}
