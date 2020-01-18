using System;
using UIKit;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.iOS.Handlers
{
	public class ToggleHandler : AbstractControlHandler<Toggle, UISwitch>
	{
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle>(ViewHandler.Mapper)
		{
			[nameof(Toggle.IsOn)] = MapIsOnProperty
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

		public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
		{
			var nativeView = (UISwitch)viewHandler.NativeView;
			nativeView.On = virtualView.IsOn?.CurrentValue ?? false;
		}
	}
}
