using System;
using System.Collections.Generic;
using Comet.Samples.Comparisons;
using Comet.Samples.LiveStreamIssues;
using Comet.Samples.Skia;

namespace Comet.Samples
{
	public class MainPage : View
	{
		List<MenuItem> pages = new List<MenuItem>
		{
			new MenuItem("Ride the Comet",()=> new RideSample()),
			new MenuItem("Text Styles",()=> new TextStylesSample()),
			new MenuItem("Text Weight",()=> new TextWeightSample()),
			new MenuItem("Skia Controls Sample",()=> new SkiaControlsSample()),
			new MenuItem("Material Design",()=> new MaterialStylePicker()),
			new MenuItem("AuditReportPage",()=> new AuditReportPage()),
			new MenuItem("VStackSample",()=> new VStackSample()),
			new MenuItem("Demo Credit Card",()=> new DemoCreditCardView()),
			new MenuItem("Demo Credit Card 2",()=> new DemoCreditCardView2()),
			new MenuItem("Demo Credit Card 3",()=> new DemoCreditCardView3()),
			new MenuItem("Sectioned List View",()=>new SectionedListViewSample()),
			new MenuItem("Virtual Sectioned List View",()=>new VirtualSectionedListViewSample()),
			new MenuItem("Virtual List View",()=>new VirtualListViewSample()),
			new MenuItem("Binding Sample!",()=> new BindingSample()),
			new MenuItem("Animated Skia Sample",()=> new SkiaButtonSample()),
			new MenuItem("Nested View", ()=> new NestedViews()),
			new MenuItem("Animation Sample",()=> new AnimationSample()),
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
			new MenuItem("ProgressBarSample1", ()=> new ProgressBarSample1()),
			new MenuItem("ProgressBarSample2", ()=> new ProgressBarSample2()),
			new MenuItem("SecureFieldSample1", ()=> new SecureFieldSample1()),
			new MenuItem("SecureFieldSample2", ()=> new SecureFieldSample2()),
			new MenuItem("SecureFieldSample3", ()=> new SecureFieldSample3()),
			new MenuItem("ShapeSample1", ()=> new ShapeSample1()),
			new MenuItem("ShapeSample2", ()=> new ShapeSample2()),
			new MenuItem("SliderSample1", ()=> new SliderSample1()),
			new MenuItem("StepperSample1", ()=> new StepperSample1()),
			new MenuItem("DatePickerSample", ()=> new DatePickerSample()),
			new MenuItem("TextFieldSample1", ()=> new TextFieldSample1()),
			new MenuItem("TextFieldSample2", ()=> new TextFieldSample2()),
			new MenuItem("TextFieldSample3", ()=> new TextFieldSample3()),
			new MenuItem("TextFieldSample4", ()=> new TextFieldSample4()),
			new MenuItem("RadioButtonSample", ()=> new RadioButtonSample()),
			new MenuItem("SkiaSample1 (FingerPaint)", ()=> new SkiaSample1()),
			new MenuItem("SkiaSample2 (FingerPaint)", ()=> new SkiaSample2()),
			new MenuItem("SkiaSample3 (BindableFingerPaint)", ()=> new SkiaSample3()),
			new MenuItem("SkiaSample3WithScrollView (BindableFingerPaint)", ()=> new SkiaSample3WithScrollView()),
			new MenuItem("SkiaSample4 (BindableFingerPaint)", ()=> new SkiaSample4()),
			new MenuItem("SkiaSample5 (Shapes) ",() => new SkiaSample5()),
			new MenuItem("SkiaSample6 (Shapes w/ Gradient) ",() => new SkiaSample6()),
			new MenuItem("SwiftUI Tutorial Section 1", ()=> new Section1()),
			new MenuItem("SwiftUI Tutorial Section 2", ()=> new Section2()),
			new MenuItem("SwiftUI Tutorial Section 3", ()=> new Section3()),
			new MenuItem("SwiftUI Tutorial Section 4", ()=> new Section4()),
			new MenuItem("SwiftUI Tutorial Section 4b", ()=> new Section4b()),
			new MenuItem("SwiftUI Tutorial Section 4c", ()=> new Section4c()),
			new MenuItem("SwiftUI Tutorial Section 4d", ()=> new Section4c()),
			new MenuItem("DavidSample1",()=> new DavidSample1()),
			new MenuItem("DavidSample1a",()=> new DavidSample1a()),
			new MenuItem("DavidSample1b",()=> new DavidSample1b()),
			new MenuItem("DavidSample1c",()=> new DavidSample1c()),
			new MenuItem("DavidSample2",()=> new DavidSample2()),
			new MenuItem("Issue123",() => new Issue123()),
			new MenuItem("Issue125",() => new Issue125()),
			new MenuItem("Issue125b",() => new Issue125b()),
			new MenuItem("Issue125c",() => new Issue125c()),
			new MenuItem("Issue133",() => new Issue133()),
			new MenuItem("Issue133b",() => new Issue133b()),
			new MenuItem("Issue133c",() => new Issue133c()),
			new MenuItem("Question1",() => new Question1()),
			new MenuItem("Question1a",() => new Question1a()),
			new MenuItem("Question1b",() => new Question1b()),
			new MenuItem("Question1c",() => new Question1c()),
			new MenuItem("Question1d",() => new Question1d()),
			new MenuItem("Question1e",() => new Question1e()),
		};


		public MainPage(List<MenuItem> additionalPage = null)
		{
			//This is only required since there is a parameter for the view
			HotReloadHelper.Register(this, additionalPage);
			if (additionalPage != null)
				pages.AddRange(additionalPage);

			this.Title("UI Samples");

			Body = () => new NavigationView
			{
				new ListView<MenuItem> (pages)
				{
					ViewFor = (page) =>  new HStack()
					{
						new Text(page.Title),
						new Spacer()
					}.Frame(height:44).Margin(left:10),
					//ViewFor = (page) => new Text(page.Title).FillHorizontal().TextAlignment(TextAlignment.Left).Frame(height:44).Margin(left:10),
				}.OnSelectedNavigate( page => page.Page().Title(page.Title))
			};
		}
	}
}
