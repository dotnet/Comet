using Comet.Blazor.Handlers;
using Microsoft.AspNetCore.Builder;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Comet.Blazor.Tests
{
    public class RegistrarTests
    {
        private readonly ITestOutputHelper _output;

        public RegistrarTests(ITestOutputHelper output)
        {
            _output = output;
        }

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


        static List<string> skippedViews = new List<string>
        {
            nameof(Section),
            //The Generic <T>
            $"{nameof(Section)}`1",
        };
        private void AllInternalHandlersAreRegistered(Assembly assembly)
        {
            var appBuilder = GetAppBuilder();

            appBuilder.UseComet();

            var types = assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(View).IsAssignableFrom(t));

            foreach (var type in types)
            {
                if (skippedViews.Contains(type.Name))
                    continue;
                var handler = Registrar.Handlers.GetHandler(type);

                Assert.NotNull(handler);

                if (type == typeof(View))
                {
                    Assert.IsType<ViewHandler>(handler);
                }
                else
                {
                    _output.WriteLine(type.FullName);
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
