using System;
using UIKit;

// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.iOS.Handlers
{
	public class ToggleHandler : AbstractControlHandler<Switch, UISwitch>
	{
		public static readonly PropertyMapper<Switch> Mapper = new PropertyMapper<Switch>(ViewHandler.Mapper)
		{
			[nameof(Switch.IsOn)] = MapIsOnProperty
		};

		public ToggleHandler() : base(Mapper)
		{
		}

		protected override UISwitch CreateView()
		{
			var toggle = new UISwitch();
			toggle.ValueChanged += HandleValueChanged;
			return toggle;
		}

		protected override void DisposeView(UISwitch toggle)
		{
			toggle.ValueChanged -= HandleValueChanged;
		}

		void HandleValueChanged(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.On);

		public static void MapIsOnProperty(IViewHandler viewHandler, Switch virtualView)
		{
			var nativeView = (UISwitch)viewHandler.NativeView;
			nativeView.On = virtualView.IsOn?.CurrentValue ?? false;
		}
	}
}
