using System;
using System.Drawing;
using Comet.iOS.Handlers;
using CoreGraphics;
using UIKit;

namespace Comet.iOS
{
	public class DatePickerHandler : AbstractControlHandler<DatePicker, NoCaretField>
	{
		static UIDatePicker _picker;
		static NoCaretField _dateField;
		
		public static readonly PropertyMapper<DatePicker> Mapper = new PropertyMapper<DatePicker>(ViewHandler.Mapper)
		{
			[nameof(DatePicker.Date)] = MapDateProperty,
			[nameof(DatePicker.MaximumDate)] = MapMaximumDateProperty,
			[nameof(DatePicker.MinimumDate)] = MapMinimumDateProperty,
			[nameof(DatePicker.Format)] = MapFormatProperty
		};

		public DatePickerHandler() : base(Mapper)
		{
		}
		
		protected override NoCaretField CreateView()
		{
			_dateField = new NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };
			_picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new Foundation.NSTimeZone("UTC") };
			_picker.ValueChanged += HandleValueChanged;

			var width = (float)UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new RectangleF(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => _dateField.ResignFirstResponder());

			toolbar.SetItems(new[] { spacer, doneButton }, false);

			_dateField.InputView = _picker;
			_dateField.InputAccessoryView = toolbar;

			_dateField.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
			_dateField.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			_dateField.InputAssistantItem.LeadingBarButtonGroups = null;
			_dateField.InputAssistantItem.TrailingBarButtonGroups = null;

			_dateField.AccessibilityTraits = UIAccessibilityTrait.Button;
			return _dateField;
		}

		private void HandleValueChanged(object sender, EventArgs e)
		{
			_dateField.Text = _picker.Date.ToDateTime().Date.ToString(VirtualView?.Format.CurrentValue);
			VirtualView?.OnDateChanged.Invoke(_picker.Date.ToDateTime().Date);
		}

		public static void MapDateProperty(IViewHandler viewHandler, DatePicker virtualView)
		{
			if(virtualView.Date != null)
				_picker.Date = virtualView.Date.CurrentValue.ToNSDate();
		}

		public static void MapMaximumDateProperty(IViewHandler viewHandler, DatePicker virtualView)
		{
			_picker.MaximumDate = virtualView.MaximumDate?.CurrentValue.ToNSDate();
		}

		public static void MapMinimumDateProperty(IViewHandler viewHandler, DatePicker virtualView)
		{
			_picker.MinimumDate = virtualView.MinimumDate?.CurrentValue.ToNSDate();
		}


		public static void MapFormatProperty(IViewHandler viewHandler, DatePicker virtualView)
		{
			if (_dateField.Text != null && virtualView.Format != null)
				_dateField.Text = _picker.Date.ToDateTime().ToString(virtualView.Format.CurrentValue);
		}

		protected override void DisposeView(NoCaretField nativeView)
		{
			if(_picker != null)
			{
				_picker.RemoveFromSuperview();
				_picker.ValueChanged -= HandleValueChanged;
				_picker.Dispose();
				_picker = null;
			}
		}
	}

	public class NoCaretField : UITextField
	{
		public NoCaretField() : base(new RectangleF())
		{
			SpellCheckingType = UITextSpellCheckingType.No;
			AutocorrectionType = UITextAutocorrectionType.No;
			AutocapitalizationType = UITextAutocapitalizationType.None;
		}

		public override CGRect GetCaretRectForPosition(UITextPosition position)
		{
			return new CGRect();
		}
	}
}
