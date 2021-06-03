using System;
using UIKit;

namespace Comet.iOS
{
	class CUIRadioButton : UIButton
	{
		private const float CONTENT_SPACING = 10;

		private static UIImage SelectedImage = UIImage.GetSystemImage("largecircle.fill.circle")
			.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
		private static UIImage DeselectedImage = UIImage.GetSystemImage("circle")
			.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

		public event EventHandler IsCheckedChanged;

		public CUIRadioButton(bool isChecked = false)
		{
			SetImage(_isChecked ? SelectedImage : DeselectedImage, UIControlState.Normal);
			IsChecked = isChecked;

			SetTitleColor(UIColor.Blue, UIControlState.Normal);

			ContentEdgeInsets = new UIEdgeInsets(0, CONTENT_SPACING, 0, CONTENT_SPACING);
			TitleEdgeInsets = new UIEdgeInsets(0, CONTENT_SPACING, 0, -CONTENT_SPACING);

			TouchUpInside += (sender, e) => { if (!IsChecked) IsChecked = true; };
		}

		public override void SetTitleColor(UIColor color, UIControlState forState)
		{
			base.SetTitleColor(color, forState);

			if (forState == UIControlState.Normal)
			{
				ImageView.TintColor = color;
			}
		}

		private bool _isChecked = false;

		public bool IsChecked
		{
			get => _isChecked;
			set
			{
				if (_isChecked != value)
				{
					_isChecked = value;
					SetImage(_isChecked ? SelectedImage : DeselectedImage, UIControlState.Normal);
					IsCheckedChanged?.Invoke(this, new EventArgs());
				}
			}
		}
	}
}