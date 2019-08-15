using Comet.Blazor.Handlers;
using Microsoft.AspNetCore.Builder;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Comet.Blazor.Tests
{
    public class RegistrarTests
    {
        [Fact]
        public void AllHandlersAreRegistered()
        {
            var appBuilder = GetAppBuilder();

            appBuilder.UseComet();

            var types = typeof(IBlazorViewHandler).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.ContainsGenericParameters && typeof(IBlazorViewHandler).IsAssignableFrom(t));

            foreach (var type in types)
            {
                var expectedHandler = (IBlazorViewHandler)Activator.CreateInstance(type);

                var actualHandler = Registrar.Handlers.GetHandler(expectedHandler.VirtualType);

                Assert.IsType(expectedHandler.GetType(), actualHandler);
            }
        }

        [Fact]
        public void AllCometHandlersRegistered() => AllInternalHandlersAreRegistered(typeof(View).Assembly);

        [Fact]
        public void AllCometSkiaHandlersRegistered() => AllInternalHandlersAreRegistered(typeof(Skia.SkiaShapeView).Assembly);

        private void AllInternalHandlersAreRegistered(Assembly assembly)
        {
            var appBuilder = GetAppBuilder();

            appBuilder.UseComet();

            var types = assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(View).IsAssignableFrom(t));

            foreach (var type in types)
            {
                var handler = Registrar.Handlers.GetHandler(type);

                Assert.NotNull(handler);

                if (type == typeof(View))
                {
                    Assert.IsType<ViewHandler>(handler);
                }
                else
                {
                    Assert.IsNotType<ViewHandler>(handler);
                }
            }
        }

        private static IApplicationBuilder GetAppBuilder()
        {
            var appBuilder = Substitute.For<IApplicationBuilder>();
            var services = appBuilder.ApplicationServices;

            services.GetService(Arg.Any<Type>()).Returns(info =>
            {
                return Substitute.For(new[] { info.Arg<Type>() }, Array.Empty<object>());
            });

            return appBuilder;
        }
    }
}
