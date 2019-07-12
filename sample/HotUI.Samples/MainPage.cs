using System;
using System.Collections.Generic;
using HotUI.Samples.Comparisons;

namespace HotUI.Samples {
	public class MainPage : View {
		List<MenuItem> pages = new List<MenuItem> {
			new MenuItem("Binding Sample!",()=> new BindingSample()),
            new MenuItem("BasicTestView",()=> new BasicTestView()),
            new MenuItem("ListPage1", ()=> new ListPage()),
            new MenuItem("ListPage2", ()=> new ListPage2()),
            new MenuItem("Insane Diff", ()=> new InsaneDiffPage()),
            new MenuItem("ButtonSample1", ()=> new ButtonSample1()),
            new MenuItem("SecureFieldSample1", ()=> new SecureFieldSample1()),
            new MenuItem("SecureFieldSample2", ()=> new SecureFieldSample2()),
            new MenuItem("SecureFieldSample3", ()=> new SecureFieldSample3()),
            new MenuItem("SliderSample1", ()=> new SliderSample1()),
            new MenuItem("TextFieldSample1", ()=> new TextFieldSample1()),
            new MenuItem("SwiftUI Tutorial Section 1", ()=> new Section1()),
            new MenuItem("SwiftUI Tutorial Section 2", ()=> new Section2()),
            new MenuItem("SwiftUI Tutorial Section 3", ()=> new Section3()),
            new MenuItem("SwiftUI Tutorial Section 4", ()=> new Section4()),
            new MenuItem("SwiftUI Tutorial Section 4b", ()=> new Section4b()),
            new MenuItem("SwiftUI Tutorial Section 4c", ()=> new Section4c()),
        };
		public MainPage (List<MenuItem> additionalPage = null)
		{
            //This is only required since there is a parameter for the view
            HotReloadHelper.Register(this, additionalPage);
            if (additionalPage != null)
                pages.AddRange(additionalPage);

			Body = () => new NavigationView {
				new ListView<MenuItem> (pages) {
					Cell = (page) => new NavigationButton (page.Title,page.Page),
				}
			};
		}

	}
}