using System;
using AppKit;

// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.Mac.Handlers
{
	public class ToggleHandler : AbstractControlHandler<Switch, NSButton>
	{
		public static readonly PropertyMapper<Switch> Mapper = new PropertyMapper<Switch>(ViewHandler.Mapper)
		{
			[nameof(Switch.IsOn)] = MapIsOnProperty
		};

		public ToggleHandler() : base(Mapper)
		{
		}

		protected override NSButton CreateView()
		{
			var toggle = new NSButton();
			toggle.SetButtonType(NSButtonType.Switch);
			toggle.Activated += HandleValueChanged;
			return toggle;
		}

		protected override void DisposeView(NSButton toggle)
		{
			toggle.Activated -= HandleValueChanged;
		}

		void HandleValueChanged(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.State == NSCellStateValue.On);

		public static void MapIsOnProperty(IViewHandler viewHandler, Switch virtualView)
		{
			var nativeView = (NSButton)viewHandler.NativeView;
			nativeView.State = virtualView.IsOn ? NSCellStateValue.On : NSCellStateValue.Off;
		}
	}
}
