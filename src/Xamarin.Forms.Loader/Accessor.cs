using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using Xamarin.Forms.Internals;
using Xamarin.Forms.StyleSheets;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace System.Maui.Internal
{

	public static class BindingExpression
	{
		public static bool TryConvert(ref object value, BindableProperty targetProperty, Type convertTo, bool toTarget)
			=> Xamarin.Forms.BindingExpression.TryConvert(ref value, targetProperty, convertTo, toTarget);
	}
	/// <summary>
	/// This library exists solely to satisfy XF's InternalsVisibleTo which requires 
	/// it to be named Xamarin.Forms.Loader in order to be able to set the 
	/// <see cref="ResourceLoader.ResourceProvider"/> and access the <see cref="XamlLoader"/>.
	/// </summary>
	public static class Accessor
	{

		static readonly IList<StyleSheet> emptyStyleSheets = new List<StyleSheet>();

		// Resource dictionary instances (those coming from XamlG/C compilation) 
		// are cached by their type, so we need to clear the cache in order for reloading to work :(
		// See https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Core/ResourceDictionary.cs#L19
		// By querying this way, we are at least resilient to field renaming.
		static readonly FieldInfo resourceDictionaryCache = typeof(ResourceDictionary)
			.GetFields(BindingFlags.Static | BindingFlags.NonPublic)
			.FirstOrDefault(x => x.FieldType == typeof(ConditionalWeakTable<Type, ResourceDictionary>));

		/// <summary>
		/// Sets the <see cref="DesignMode.IsDesignModeEnabled"/> property 
		/// which causes live reload to skip initialization.
		/// </summary>
		/// <param name="designMode"></param>
		public static void SetDesignMode(bool designMode)
			=> DesignMode.IsDesignModeEnabled = designMode;

		/// <summary>
		/// Gets the stylesheets in the resource dictionary.
		/// </summary>
		public static IList<StyleSheet> GetStyleSheets(ResourceDictionary resources)
			=> resources.StyleSheets ?? emptyStyleSheets;

		/// <summary>
		/// Override the built-in resource loader with the given one.
		/// </summary>
		public static Func<AssemblyName, string, string> ResourceProvider
		{
			get => ResourceLoader.ResourceProvider;
			set
			{

				ResourceLoader.ResourceProvider = value;
				// Setting the ResourceProvider causes the DesignMode to be set
				// as a side-effect. So revert that effect right-away.
				DesignMode.IsDesignModeEnabled = false;
			}
		}

		/// <summary>
		/// Resets the internal cache for the given resource dictionary type.
		/// </summary>
		public static void ResetResourceDictionary(Type resourceType, bool throwIfMissing = false)
		{
			// Resource dictionary instances (those coming from XamlG/C compilation) 
			// are cached by their type, so we need to clear the cache in order for reloading to work :(
			// See https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Core/ResourceDictionary.cs#L19
			if (typeof(ResourceDictionary).IsAssignableFrom(resourceType))
			{
				// By querying this way, we are at least resilient to field renaming.
				var field = typeof(ResourceDictionary).GetFields(BindingFlags.Static | BindingFlags.NonPublic)
					.FirstOrDefault(x => x.FieldType == typeof(ConditionalWeakTable<Type, ResourceDictionary>));

				if (field == null && throwIfMissing)
					throw new InvalidOperationException("Could not find the internal resource dictionary cache.");

				// Report to the debugger if this is broken for some reason.
				Debug.Assert(field != null, "Xamarin.Forms caching of ResourceDictionary changed. Reloading them will not work. Please contact the Xamarin Live Reload team.");

				if (field != null)
				{
					var value = (ConditionalWeakTable<Type, ResourceDictionary>)field.GetValue(null);
					value.Remove(resourceType);
				}
			}
		}

		/// <summary>
		/// Sets the internal cache for the given resource dictionary type to the specified instance.
		/// </summary>
		public static void SetResourceDictionary(ResourceDictionary instance, bool throwIfMissing = false)
		{
			if (resourceDictionaryCache == null && throwIfMissing)
				throw new InvalidOperationException("Could not find the internal resource dictionary cache.");

			// Report to the debugger if this is broken for some reason.
			Debug.Assert(resourceDictionaryCache != null, "Xamarin.Forms caching of ResourceDictionary changed. Reloading them will not work. Please contact the Xamarin Live Reload team.");

			if (resourceDictionaryCache != null)
			{
				var value = (ConditionalWeakTable<Type, ResourceDictionary>)resourceDictionaryCache.GetValue(null);
				lock (value)
				{
					value.Remove(instance.GetType());
					value.Add(instance.GetType(), instance);
				}
			}
		}

		/// <summary>
		/// Creates an object from its XAML representation.
		/// </summary>
		/// <returns>The created object from the XAML, or <see langword="null"/> if Xamarin.Forms does not support this anymore.</returns>
		/// <exception cref="Xamarin.Forms.Xaml.XamlParseException">The XAML is not valid.</exception>
		public static object CreateFromXaml(string xaml)
		{
			object root = null;

			using (var xml = XmlReader.Create(new StringReader(xaml)))
			{
				xml.MoveToContent();
				var rootnode = new RuntimeRootNode(
					new XmlType(xml.NamespaceURI, xml.Name, null),
					(IXmlNamespaceResolver)xml);

				XamlParser.ParseXaml(rootnode, xml);
				var visitorContext = new HydrationContext();
				new CreateValuesVisitor(visitorContext).Visit((ElementNode)rootnode, null);

				root = visitorContext.Values[rootnode];
			}

			root.LoadFromXaml(xaml);

			return root;
		}

		class RuntimeRootNode : RootNode
		{
			public RuntimeRootNode(XmlType xmlType, IXmlNamespaceResolver resolver)
				: base(xmlType, resolver)
			{
			}
		}
	}
}
