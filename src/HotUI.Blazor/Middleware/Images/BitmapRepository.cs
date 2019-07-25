using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;

namespace HotUI.Blazor.Middleware.Images
{
    internal class BitmapRepository
    {
        private readonly ConcurrentDictionary<string, string> _repository;

        public BitmapRepository()
        {
            _repository = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);
        }

        public void Add(BlazorBitmap bitmap)
        {
            _repository.TryAdd(bitmap.Id, bitmap.Url);
        }

        public void Remove(BlazorBitmap bitmap)
        {
            _repository.TryRemove(bitmap.Id, out _);
        }

        public bool TryMatch(PathString path, out string url)
        {
            return _repository.TryGetValue(path.Value.Substring(1), out url);
        }
    }
}
