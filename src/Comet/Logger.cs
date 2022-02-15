using System;
using Microsoft.Extensions.Logging;

namespace Comet
{
	class ConsoleLogger : ILogger
	{
		IDisposable ILogger.BeginScope<TState>(TState state) => null;
		bool ILogger.IsEnabled(LogLevel logLevel) => true;
		void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => Console.WriteLine(formatter(state,exception));
	}
	public static class Logger
	{
		private static ILogger _registeredService;

		public static ILogger RegisteredService
		{
			get
			{
				if (_registeredService == null)
				{
					_registeredService = ServiceContainer.Resolve<ILogger>(true);
					if (_registeredService == null)
					{
						_registeredService = new ConsoleLogger();
						_registeredService.Log(LogLevel.Warning, "No logging service was registered.  Falling back to console logging.");
					}
				}

				return _registeredService;
			}
		}

		public static void RegisterService(ILogger service)
		{
			ServiceContainer.Register(service);
			_registeredService = service;
		}

		public static void Debug(params object[] parameters)
		{
			Log(LogLevel.Debug, parameters);
		}

		public static void Warn(params object[] parameters)
		{
			Log(LogLevel.Warning, parameters);
		}

		public static void Error(params object[] parameters)
		{
			Log(LogLevel.Error, parameters);
		}

		public static void Fatal(params object[] parameters)
		{
			Log(LogLevel.Critical, parameters);
		}

		public static void Info(params object[] parameters)
		{
			Log(LogLevel.Information, parameters);
		}

		public static void Log(LogLevel LogLevel, params object[] parameters)
		{
			if (parameters == null || parameters.Length == 0)
				return;

			if (parameters.Length == 1)
			{
				if (parameters[0] is Exception exception)
				{
					RegisteredService.Log(LogLevel, exception.Message, exception);
					return;
				}

				var value = parameters[0];
				if (value != null)
				{
					RegisteredService.Log(LogLevel, value.ToString());
					return;
				}
			}

			var format = parameters[0] != null ? parameters[0].ToString() : "";
			var message = format;

			try
			{
				var args = new object[parameters.Length - 1];
				Array.Copy(parameters, 1, args, 0, parameters.Length - 1);

				message = string.Format(format, args);
			}
			catch (Exception exc)
			{
				RegisteredService.Log(LogLevel.Information, $"An error occured formatting the logging message: [{format}]", exc);
			}

			if (parameters[parameters.Length - 1] is Exception ex)
			{
				RegisteredService.Log(LogLevel, message, ex);
			}
			else
			{
				RegisteredService.Log(LogLevel, message);
			}
		}
	}
}
