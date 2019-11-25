using Comet.Graphics;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Drawing;

namespace Comet.Blazor
{
    internal class BlazorBitmap : Bitmap
    {
        public static readonly PathString Prefix = "/Comet/blazor/image";

        private readonly Action<BlazorBitmap> _remove;

        public BlazorBitmap(string url, Action<BlazorBitmap> remove)
        {
            Url = url;
            Id = GenerateId();
            _remove = remove;
        }

        public string LocalUrl => $"{Prefix}/{Id}";

        public string Id { get; }

        public string Url { get; }

        public override SizeF Size => default;

        public override object NativeBitmap => LocalUrl;

        protected override void DisposeNative()
        {
            _remove(this);
        }

        private static string GenerateId()
        {
            var guid = Guid.NewGuid();
            Span<byte> bytes = stackalloc byte[16];
            Debug.Assert(guid.TryWriteBytes(bytes));

            // Remove any padding done with '='
            return Convert.ToBase64String(bytes).TrimEnd('=');
        }
    }
}
