using System;
using System.Collections.Generic;
using System.Maui.Reflection;
using System.Maui.Internal;
using System.Linq;
using System.Reflection;

namespace System.Maui.Samples
{
	public class ApiAuditManager
	{
		static string[] IgnoredProperties =
		{
			nameof(View.Id),
			nameof(View.Parent),
			nameof(View.Navigation),
			"State",
			nameof(View.ViewHandler),
			nameof(View.Body),
			nameof(View.BuiltView),
		};

		static string[] FontProperties =
		{
			EnvironmentKeys.Fonts.Family,
			EnvironmentKeys.Fonts.Italic,
			EnvironmentKeys.Fonts.Size,
			EnvironmentKeys.Fonts.Weight,
			EnvironmentKeys.Colors.Color,
		};

		static string[] ViewProperties =
		{
			EnvironmentKeys.Colors.BackgroundColor,
			EnvironmentKeys.View.ClipShape,
			EnvironmentKeys.View.Overlay,
			EnvironmentKeys.View.Shadow,
		};

		static ApiAuditManager()
		{
			Register<View>(ViewProperties);
			Register<Label>(FontProperties);
			Register<Button>(FontProperties);
		}

		static Dictionary<Type, HashSet<string>> WatchedProperties = new Dictionary<Type, HashSet<string>>();
		public static void Register<T>(params string[] properties) where T : View
		{
			var type = typeof(T);
			if (!WatchedProperties.TryGetValue(type, out var propList))
			{
				WatchedProperties[type] = propList = new HashSet<string>();
				var baseProps = type.GetDeepProperties(BindingFlags.Public | BindingFlags.Instance);
				foreach (var prop in baseProps)
				{
					if (prop.PropertyType.Name.StartsWith("Action", StringComparison.OrdinalIgnoreCase))
						continue;
					if (!IgnoredProperties.Contains(prop.Name))
						propList.Add(prop.Name);
				}
			}

			foreach (var prop in properties)
				propList.Add(prop);
		}

		public static List<AuditReport> GenerateReport()
		{
			var reports = new List<AuditReport>();
			var pairs = Registrar.Handlers.GetAllRenderers();
			foreach (var pair in pairs)
			{
				reports.Add(GenerateReport(pair.Key, pair.Value));
			}
			return reports.OrderByDescending(x => x.UnHandledProperties.Count).ToList();
		}

		static AuditReport GenerateReport(Type viewType, Type handler)
		{
			var report = new AuditReport
			{
				View = viewType.Name,
				Handler = handler.FullName,
			};

			if (viewType.BaseType != null)
			{
				var baseHandler = Registrar.Handlers.GetRendererType(viewType);
				if (baseHandler != null)
				{
					var baseReport = GenerateReport(viewType.BaseType, baseHandler);
					foreach (var p in baseReport.HandledProperties)
					{
						if (!report.HandledProperties.Contains(p))
							report.HandledProperties.Add(p);
					}
					foreach (var p in baseReport.UnHandledProperties)
					{
						if (!report.UnHandledProperties.Contains(p))
							report.UnHandledProperties.Add(p);
					}
				}
			}


			WatchedProperties.TryGetValue(viewType, out var watchedProps);

			var mapper = handler.GetField("Mapper", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
			if (mapper != null)
			{
				var map = mapper.GetValue(null);
				var mapKeysProp = map.GetType().GetProperty("Keys", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				var keys = (mapKeysProp.GetValue(map) as IEnumerable<string>).ToList();

				if (watchedProps != null)
				{
					foreach (var prop in watchedProps)
					{
						if (keys.Contains(prop))
						{
							if (!report.HandledProperties.Contains(prop))
								report.HandledProperties.Add(prop);
							if (report.UnHandledProperties.Contains(prop))
								report.UnHandledProperties.Remove(prop);
						}
						else if (!report.HandledProperties.Contains(prop))
							report.UnHandledProperties.Add(prop);
					}
				}
				foreach (var key in keys)
				{
					if (!report.HandledProperties.Contains(key))
						report.HandledProperties.Contains(key);
				}

			}
			else
			{
				if (watchedProps != null)
				{
					foreach (var prop in watchedProps)
					{

						if (!report.HandledProperties.Contains(prop))
							report.UnHandledProperties.Add(prop);
					}
				}
				report.MissingMapper = true;
			}
			report.HandledProperties = report.HandledProperties.Distinct().ToList();
			report.UnHandledProperties = report.UnHandledProperties.Distinct().ToList();
			return report;

		}

		public class AuditReport
		{
			public string View { get; set; }
			public string Handler { get; set; }
			public List<string> HandledProperties { get; set; } = new List<string>();
			public List<string> UnHandledProperties { get; set; } = new List<string>();
			public bool MissingMapper { get; set; }
			public bool IsComplete => HandledProperties?.Count > 0 && UnHandledProperties?.Count <= 0;
		}
	}
}
