using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Maui.Samples
{
	public class AuditReportPage : View
	{
		readonly State<List<ApiAuditManager.AuditReport>> reports = new List<ApiAuditManager.AuditReport>();
		readonly State<bool> isLoading = false;

		[Body]
		View body()
		{
			//if (isLoading) return new Text(() => "Loading...");
			if (isLoading) return new ActivityIndicator().Color(Color.Fuchsia);

			if (reports.Value.Count == 0) return new Button(() => "Generate Report", async () => {
				isLoading.Value = true;
				try
				{
					var items = await Task.Run(() => ApiAuditManager.GenerateReport());
					reports.Value = items;
				}
				finally
				{
					isLoading.Value = false;
				}
			});

			return new ListView<ApiAuditManager.AuditReport>(reports.Value)
			{
				ViewFor = (report) => new HStack()
					{
						new VStack(HorizontalAlignment.Leading)
						{
							new Label (report.View).FontSize(20),
							new Label ($"Handler: {report.Handler}"),
							new Label ($"Has Map? : {!report.MissingMapper}").Color(report.MissingMapper ? Color.Red : Color.Green),
							new Label ($"Handled Properties: {report.HandledProperties.Count}").Color(report.HandledProperties.Count == 0 ? Color.Red : Color.Green),
							new Label ($"Missing Count: {report.UnHandledProperties.Count}").Color(report.UnHandledProperties.Count == 0 ? Color.Green : Color.Red),
						}.Margin().FontSize(10).OnTapNavigate(()=>new AuditReportPageDetails().SetEnvironment("report", report))
				 },
			}.OnSelectedNavigate((report) => new AuditReportPageDetails().SetEnvironment("report", report)); ;
		}
	}
	public class AuditReportPageDetails : View
	{
		[Environment]
		readonly ApiAuditManager.AuditReport report;
		[Body]
		View body()
		{
			var stack = new VStack
			{

			};//.Frame(alignment:Alignment.Top);
			if (report.HandledProperties.Count > 0)
			{
				stack.Add(new Label ("Handled Properties").FontSize(30));
				foreach (var prop in report.HandledProperties)
				{
					stack.Add(new Label (prop).Color(Color.Green));
				}
			}
			if (report.UnHandledProperties.Count > 0)
			{
				stack.Add(new Label ("UnHandled Properties!").FontSize(30));
				foreach (var prop in report.UnHandledProperties)
				{
					stack.Add(new Label (prop).Color(Color.Red));
				}
			}
			return stack;
		}
	}
}
