using AppKit;

namespace HotUI.Mac.Extensions
{
    public static partial class MacExtensions
    {
        static MacExtensions()
        {
            UI.Init();
        }

        public static NSViewController ToViewController(this View view) => new HotUIViewController {
				CurrentView = view.ToINSView (),
			};

		public static NSView ToView (this View view) => view?.ToINSView ()?.View;

		public static INSView ToINSView(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {
				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}

			return handler as INSView;
		}

	}
}