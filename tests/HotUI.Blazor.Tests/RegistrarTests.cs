using HotUI.Blazor.Handlers;
using Microsoft.AspNetCore.Builder;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace HotUI.Blazor.Tests
{
    public class RegistrarTests
    {
        [Fact]
        public void AllHandlersAreRegistered()
        {
            var appBuilder = GetAppBuilder();

            appBuilder.UseHotUI();

            var types = typeof(IBlazorViewHandler).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(IBlazorViewHandler).IsAssignableFrom(t));

            foreach (var type in types)
            {
                var expectedHandler = (IBlazorViewHandler)Activator.CreateInstance(type);

                var actualHandler = Registrar.Handlers.GetHandler(expectedHandler.VirtualType);

                Assert.IsType(expectedHandler.GetType(), actualHandler);
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
