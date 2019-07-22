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
                Cell = (report) => new HStack()
                    {
                        new VStack(HorizontalAlignment.Leading)
                        {
                            new Text (report.View)
                                .Font(Font.System(20)),
                            new Text ($"Handler: {report.Handler}"),
                            new Text ($"Has Map? : {!report.MissingMapper}").Color(report.MissingMapper ? Color.Red : Color.Green),
                            new Text ($"Handled Properties: {report.HandledProperties.Count}").Color(report.HandledProperties.Count == 0 ? Color.Red : Color.Green),
                            new Text ($"Missing Count: {report.UnHandledProperties.Count}").Color(report.UnHandledProperties.Count == 0 ? Color.Green : Color.Red),
                        }.Padding().Font(Font.System(10)).Navigate(()=>new AuditReportPageDetails().SetEnvironment("report", report))
                 },
            }.Navigate((report) => new AuditReportPageDetails().SetEnvironment("report", report)); ;
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
                stack.Add(new Text("Handled Properties").FontSize(30));
                foreach (var prop in report.HandledProperties)
                {
                    stack.Add(new Text(prop).Color(Color.Green));
                }
            }
            if (report.UnHandledProperties.Count > 0)
            {
                stack.Add(new Text("UnHandled Properties!").FontSize(30));
                foreach (var prop in report.UnHandledProperties)
                {
                    stack.Add(new Text(prop).Color(Color.Red));
                }
            }
            return stack;
        }
    }
}
