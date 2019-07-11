using HotUI;
using Xamarin.Forms;
using FImage = Xamarin.Forms.Image;
using HImage = HotUI.Image;
using HView = HotUI.View;
namespace HotUI.Forms {
	public class ImageHandler : FImage, FormsViewHandler {
		public Xamarin.Forms.View View => this;
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

		public void Remove (HView view)
		{

		}

		public void SetView (HView view)
		{
			var image = view as HImage;
			if (image == null)
				return;
			this.UpdateProperties (image);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateBaseProperty (property, value);
		}
	}
}
