using System;
using System.Collections.Generic;
using HotUI.Samples.Comparisons;
using HotUI.Samples.LiveStreamIssues;
using HotUI.Samples.Skia;

namespace HotUI.Samples {
	public class MainPage : View {
		List<MenuItem> pages = new List<MenuItem> {
			new MenuItem("Binding Sample!",()=> new BindingSample()),
            new MenuItem("TabView",()=> new TabViewSample()),
            new MenuItem("BasicTestView",()=> new BasicTestView()),
            new MenuItem("ListViewSample1", ()=> new ListViewSample1()),
            new MenuItem("ListViewSample2", ()=> new ListViewSample2()),
            new MenuItem("Insane Diff", ()=> new InsaneDiffPage()),
            new MenuItem("ButtonSample1", ()=> new ButtonSample1()),
            new MenuItem("ClipSample1", ()=> new ClipSample1()),
            new MenuItem("ClipSample2", ()=> new ClipSample2()),
            new MenuItem("ClipSample_AspectFit", ()=> new ClipSample_AspectFit()),
            new MenuItem("ClipSample_AspectFill", ()=> new ClipSample_AspectFill()),
            new MenuItem("ClipSample_Fill", ()=> new ClipSample_Fill()),
            new MenuItem("ClipSample_None", ()=> new ClipSample_None()),
            new MenuItem("GridSample1", ()=> new GridSample1()),
            new MenuItem("SecureFieldSample1", ()=> new SecureFieldSample1()),
            new MenuItem("SecureFieldSample2", ()=> new SecureFieldSample2()),
            new MenuItem("SecureFieldSample3", ()=> new SecureFieldSample3()),
            new MenuItem("ShapeSample1", ()=> new ShapeSample1()),
            new MenuItem("SliderSample1", ()=> new SliderSample1()),
            new MenuItem("TextFieldSample1", ()=> new TextFieldSample1()),
            new MenuItem("TextFieldSample2", ()=> new TextFieldSample2()),
            new MenuItem("TextFieldSample3", ()=> new TextFieldSample3()),
            new MenuItem("TextFieldSample4", ()=> new TextFieldSample4()),
            new MenuItem("SkiaSample1 (FingerPaint)", ()=> new SkiaSample1()),
            new MenuItem("SkiaSample2 (FingerPaint)", ()=> new SkiaSample2()),
            new MenuItem("SkiaSample3 (BindableFingerPaint)", ()=> new SkiaSample3()),
            new MenuItem("SkiaSample4 (BindableFingerPaint)", ()=> new SkiaSample4()),
            new MenuItem("SwiftUI Tutorial Section 1", ()=> new Section1()),
            new MenuItem("SwiftUI Tutorial Section 2", ()=> new Section2()),
            new MenuItem("SwiftUI Tutorial Section 3", ()=> new Section3()),
            new MenuItem("SwiftUI Tutorial Section 4", ()=> new Section4()),
            new MenuItem("SwiftUI Tutorial Section 4b", ()=> new Section4b()),
            new MenuItem("SwiftUI Tutorial Section 4c", ()=> new Section4c()),
            new MenuItem("SwiftUI Tutorial Section 4d", ()=> new Section4c()),
            new MenuItem("AuditReportPage",()=> new AuditReportPage()),
            new MenuItem("DavidSample1",()=> new DavidSample1()),
            new MenuItem("DavidSample1a",()=> new DavidSample1a()),
            new MenuItem("DavidSample1b",()=> new DavidSample1b()),
            new MenuItem("DavidSample1c",()=> new DavidSample1c()),
            new MenuItem("DavidSample2",()=> new DavidSample2()),
        };

		public MainPage (List<MenuItem> additionalPage = null)
		{
            this.Title("Main Page");
            //This is only required since there is a parameter for the view
            HotReloadHelper.Register(this, additionalPage);
            if (additionalPage != null)
                pages.AddRange(additionalPage);

            Body = () => new NavigationView
            {
				new ListView<MenuItem> (pages)
                {
					Cell = (page) =>  new HStack()
                    {
                        new Text(page.Title),
                        new Spacer()
                    }.Frame(height:44).Padding(left:10),
				}.OnSelected(page => Navigation.PerformNavigate(page.Page?.Invoke()?.Title(page.Title)))
			};
            
        }

    }
}