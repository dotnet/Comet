using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Comet.iOS.Handlers
{
	public class StepperHandler : AbstractControlHandler<Stepper, UIStepper>
	{
		public static readonly PropertyMapper<Stepper> Mapper = new PropertyMapper<Stepper>(ViewHandler.Mapper)
		{
			[nameof(Stepper.Value)] = MapValueProperty,
			[nameof(Stepper.Maximum)] = MapMaximumProperty,
			[nameof(Stepper.Minimum)] = MapMinimumProperty,
			[nameof(Stepper.Increment)] = MapIncremntProperty,
		};


		public StepperHandler() : base(Mapper)
		{

		}

		protected override UIStepper CreateView()
		{
			var uiStepper = new UIStepper();
			uiStepper.ValueChanged += HandleValueChanged;
			return uiStepper;
		}

		private void HandleValueChanged(object sender, EventArgs e)
		{
			VirtualView?.OnValueChanged.Invoke(TypedNativeView.Value);
		}

		protected override void DisposeView(UIStepper nativeView)
		{
			nativeView.ValueChanged -= HandleValueChanged;
		}

		public static void MapValueProperty(IViewHandler viewHandler, Stepper virtualView)
		{
			var nativeView = (UIStepper)viewHandler.NativeView;
			nativeView.Value = virtualView.Value.CurrentValue;
		}

		private static void MapIncremntProperty(IViewHandler viewHandler, Stepper virtualView)
		{
			var nativeView = (UIStepper)viewHandler.NativeView;
			nativeView.StepValue = virtualView.Increment.CurrentValue;
		}

		private static void MapMinimumProperty(IViewHandler viewHandler, Stepper virtualView)
		{
			var nativeView = (UIStepper)viewHandler.NativeView;
			nativeView.MinimumValue = virtualView.Minimum.CurrentValue;
		}

		private static void MapMaximumProperty(IViewHandler viewHandler, Stepper virtualView)
		{
			var nativeView = (UIStepper)viewHandler.NativeView;
			nativeView.MaximumValue = virtualView.Maximum.CurrentValue;
		}

	}
}