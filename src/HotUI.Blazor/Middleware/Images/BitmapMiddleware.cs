using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotUI.Blazor.Middleware.Images
{
    internal class BitmapMiddleware : IMiddleware
    {
        private readonly HttpClient _client;
        private readonly BitmapRepository _repository;
        private readonly ILogger<BitmapMiddleware> _logger;

        public BitmapMiddleware(HttpClient client, BitmapRepository repository, ILogger<BitmapMiddleware> logger)
        {
            _client = client;
            _repository = repository;
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_repository.TryMatch(context.Request.Path, out var url))
            {
                return ProxyImageAsync(context, url);
            }
            else
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            }
        }

        private async Task ProxyImageAsync(HttpContext context, string url)
        {
            using (var result = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
            {
                if (!result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Could not find image at {Url}", url);
                    context.Response.StatusCode = 404;
                    return;
                }
                else
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = result.Content.Headers.ContentType.MediaType;

                    using (var stream = await result.Content.ReadAsStreamAsync())
                    {
                        await stream.CopyToAsync(context.Response.Body, context.RequestAborted).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
