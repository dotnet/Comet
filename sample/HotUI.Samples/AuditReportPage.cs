using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotUI.Samples
{
    public class AuditReportPage : View
    {
        readonly State<List<ApiAuditManager.AuditReport>> reports = new List<ApiAuditManager.AuditReport>();
        readonly State<bool> isLoading = false;

        [Body]
        View body()
        {
            if (isLoading)
                return new Text(() => "Loading...");
            if (reports.Value.Count == 0)
                return new Button(() => "Generate Report", async () =>
                   {
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
                Cell = (report) => new NavigationButton(
                    () => new AuditReportPageDetails().SetEnvironment("report", report))
                {
                    new HStack {
                        new VStack {
                            new Text (report.View) 
                                .Font(Font.System(20)),
                            new Text ($"Handler: {report.Handler}"),
                            new Text ($"Has Map? : {!report.MissingMapper}"),
                            new Text ($"Handled Properties: {report.HandledProperties.Count}"),
                            new Text ($"Missing Count: {report.UnHandledProperties.Count}"),
                        }.Font(Font.System(10))
                    }
                 },
            };
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
            if(report.HandledProperties.Count > 0)
            {
                stack.Add(new Text("Handled Properties").FontSize(30));
                foreach(var prop in report.HandledProperties)
                {
                    stack.Add(new Text(prop));
                }
            }
            if(report.UnHandledProperties.Count > 0)
            {
                stack.Add(new Text("UnHandled Properties!").FontSize(30));
                foreach (var prop in report.UnHandledProperties)
                {
                    stack.Add(new Text(prop));
                }
            }
            return stack;
        }
    }
}
