using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

namespace System.Maui
{
	/// <summary>
	/// A simple service container implementation, singleton only
	/// </summary>
	public static class ServiceContainer
	{
		static readonly ConcurrentDictionary<Type, Lazy<object>> services = new ConcurrentDictionary<Type, Lazy<object>>();
		static readonly Stack<Dictionary<Type, object>> scopedServices = new Stack<Dictionary<Type, object>>();

		/// <summary>
		/// Register the specified service with an instance
		/// </summary>
		public static void Register<T>(T service)
		{
			services[typeof(T)] = new Lazy<object>(() => service);
		}

		/// <summary>
		/// Register the specified service for a class with a default constructor
		/// </summary>
		public static void Register<T>() where T : new()
		{
			services[typeof(T)] = new Lazy<object>(() => new T(), LazyThreadSafetyMode.ExecutionAndPublication);
		}

		/// <summary>
		/// Register the specified service with a callback to be invoked when requested
		/// </summary>
		public static void Register<T>(Func<T> function)
		{
			services[typeof(T)] = new Lazy<object>(() => function());
		}

		/// <summary>
		/// Register the specified service with an instance
		/// </summary>
		public static void Register(Type type, object service)
		{
			services[type] = new Lazy<object>(() => service);
		}

		/// <summary>
		/// Register the specified service with a callback to be invoked when requested
		/// </summary>
		public static void Register(Type type, Func<object> function)
		{
			services[type] = new Lazy<object>(function, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		/// <summary>
		/// Register the specified service with an instance that is scoped
		/// </summary>
		public static void RegisterScoped<T>(T service)
		{
			Dictionary<Type, object> scope;
			if (scopedServices.Count == 0)
			{
				scope = new Dictionary<Type, object>();
				scopedServices.Push(scope);
			}
			else
			{
				scope = scopedServices.Peek();
			}

			scope[typeof(T)] = service;
		}

		/// <summary>
		/// Resolves the type, throwing an exception if not found
		/// </summary>
		public static T Resolve<T>(bool nullIsAcceptable = false)
		{
			return (T)Resolve(typeof(T), nullIsAcceptable);
		}

		/// <summary>
		/// Resolves the type, throwing an exception if not found
		/// </summary>
		public static object Resolve(Type type, bool nullIsAcceptable = false)
		{
			//Scoped services
			if (scopedServices.Count > 0)
			{
				var scope = scopedServices.Peek();

				object service;
				if (scope.TryGetValue(type, out service))
				{
					return service;
				}
			}

			//Non-scoped services
			{
				Lazy<object> service;
				if (services.TryGetValue(type, out service))
				{
					return service.Value;
				}

				if (nullIsAcceptable) return null;

				throw new KeyNotFoundException(string.Format("Service not found for type '{0}'", type));
			}
		}

		/// <summary>
		/// Adds a "scope" which is a way to register a service on a stack to be popped off at a later time
		/// </summary>
		public static void AddScope()
		{
			scopedServices.Push(new Dictionary<Type, object>());
		}

		/// <summary>
		/// Removes the current "scope" which pops off some local services
		/// </summary>
		public static void RemoveScope()
		{
			if (scopedServices.Count > 0)
				scopedServices.Pop();
		}

		/// <summary>
		/// Mainly for testing, clears the entire container
		/// </summary>
		public static void Clear()
		{
			services.Clear();
			scopedServices.Clear();
		}
	}
}
