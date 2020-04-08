using System.Maui.Blazor.Handlers;
using Microsoft.AspNetCore.Builder;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace System.Maui.Blazor.Tests
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

            appBuilder.UseSystem.Maui();

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
        public void AllSystem.MauiHandlersRegistered() => AllInternalHandlersAreRegistered(typeof(View).Assembly);

        [Fact]
        public void AllSystem.MauiSkiaHandlersRegistered() => AllInternalHandlersAreRegistered(typeof(Skia.SkiaShapeView).Assembly);


        static List<string> skippedViews = new List<string>
        {
            nameof(Section),
            //The Generic <T>
            $"{nameof(Section)}`1",
        };
        private void AllInternalHandlersAreRegistered(Assembly assembly)
        {
            var appBuilder = GetAppBuilder();

            appBuilder.UseSystem.Maui();

            var types = assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(View).IsAssignableFrom(t));

            foreach (var type in types)
            {
                if (skippedViews.Contains(type.Name))
                    continue;
                var handler = Registrar.Handlers.GetHandler(type);

                Assert.NotNull(handler);
				//Skia controls don't need explicit handlers
                if (type.FullName.StartsWith("System.Maui.Skia"))
                    continue;

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
