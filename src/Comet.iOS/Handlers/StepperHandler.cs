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
            [nameof(Stepper.Value)] = MapValueProperty
        };
        public StepperHandler() :base(Mapper)
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
            throw new NotImplementedException();
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
    }
}