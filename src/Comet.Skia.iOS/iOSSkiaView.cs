using System;
using CoreGraphics;
using Foundation;
using SkiaSharp.Views.iOS;
using Comet.iOS;
using UIKit;
using System.Linq;
using System.Drawing;

namespace Comet.Skia.iOS
{
	public class iOSSkiaView : SKCanvasView, IUIKeyInput
	{
		private SkiaView _virtualView;

		public iOSSkiaView()
		{
			Opaque = false;
		}

		public iOSSkiaView(CGRect frame) : base(frame)
		{
			Opaque = false;
		}
		public SkiaView VirtualView
		{
			get => _virtualView;
			set
			{
				if (_virtualView != null)
				{
					_virtualView.Invalidated -= HandleInvalidated;
					_virtualView.NeedsLayout -= NeedsLayout;
				}

				_virtualView = value;

				if (_virtualView != null)
				{
					_virtualView.Invalidated += HandleInvalidated;
					_virtualView.NeedsLayout += NeedsLayout;
				}

				HandleInvalidated();
			}
		}

		private void NeedsLayout(object sender, EventArgs e)
		{
			SetNeedsLayout();
		}

		private void HandleInvalidated()
		{
			if (Handle == IntPtr.Zero)
				return;
			var control = _virtualView as SkiaControl;
			if (control != null)
			{
				this.AccessibilityLabel = control.AccessibilityText();
				//this.AccessibilityTraits = UIAccessibilityTrait.StaticText;
				this.IsAccessibilityElement = true;
			}
			SetNeedsDisplay();
		}

		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			if (_virtualView == null) return;
			var canvas = e.Surface.Canvas;

			canvas.Save();
			var scale = CanvasSize.Width / (float)Bounds.Width;
			canvas.Scale(scale, scale);
			_virtualView.Draw(canvas, Bounds.ToRectangleF());
			canvas.Restore();
		}

		public override CGRect Frame
		{
			get => base.Frame;
			set
			{
				base.Frame = value;
				_virtualView?.Resized(Bounds.ToRectangleF());
			}
		}
		public override bool CanBecomeFirstResponder => _virtualView is ITextFieldHandler;

		bool pressedContained = false;
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			try
			{
				if(!this.IsFirstResponder)
					this.BecomeFirstResponder();
				var viewPoints = this.GetPointsInView(evt);
				_virtualView?.StartInteraction(viewPoints);
				pressedContained = true;
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occured handling a touch event within the control.", exc);
			}
		}
		public override bool PointInside(CGPoint point, UIEvent uievent) => (_virtualView?.TouchEnabled ?? false) && base.PointInside(point, uievent);


		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			try
			{
				var viewPoints = this.GetPointsInView(evt);
				pressedContained = VirtualView?.PointsContained(viewPoints) ?? false;
				_virtualView?.DragInteraction(viewPoints);
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occured handling a touch moved event within the control.", exc);
			}
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			try
			{
				var viewPoints = this.GetPointsInView(evt);
				_virtualView?.EndInteraction(viewPoints, pressedContained);
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occured handling a touch ended event within the control.", exc);
			}
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			try
			{
				pressedContained = false;
				_virtualView?.CancelInteraction();
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occured cancelling the touches within the control.", exc);
			}
		}

		public override bool BecomeFirstResponder()
		{
			var isFirstResponder = base.BecomeFirstResponder();
			if (isFirstResponder && _virtualView is ITextFieldHandler tf)
				tf.StartInput();
			return isFirstResponder;
		}
		public override bool ResignFirstResponder()
		{
			var isFirstResponder = base.ResignFirstResponder();
			if (isFirstResponder && _virtualView is ITextFieldHandler tf)
				tf.EndInput();
			return isFirstResponder;
		}

		public void InsertText(string text) => (_virtualView as ITextFieldHandler)?.InsertText(text);

		public void DeleteBackward() => (_virtualView as ITextFieldHandler)?.Backspace();

		public bool HasText => true;

		public UITextAutocapitalizationType AutocapitalizationType { get; set; }
		public UITextAutocorrectionType AutocorrectionType { get; set; }
		public UIKeyboardType KeyboardType { get; set; }
		public UIKeyboardAppearance KeyboardAppearance { get; set; }
		public UIReturnKeyType ReturnKeyType { get; set; }
		public bool EnablesReturnKeyAutomatically { get; set; }
		public bool SecureTextEntry { get; set; }
		public UITextSpellCheckingType SpellCheckingType { get; set; }

	}
}
