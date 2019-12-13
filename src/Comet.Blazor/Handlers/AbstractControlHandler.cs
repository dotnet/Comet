using Comet.Blazor.Components;
using System;
using System.Drawing;

namespace Comet.Blazor.Handlers
{
	public abstract class AbstractControlHandler<TVirtualView, TNativeView> : IBlazorViewHandler
	  where TVirtualView : View
	  where TNativeView : CometComponentBase
	{
		private readonly PropertyMapper<TVirtualView> _mapper;


		protected AbstractControlHandler()
		{
		}

		protected AbstractControlHandler(PropertyMapper<TVirtualView> mapper)
		{
			_mapper = mapper;
		}

		object IViewHandler.NativeView => NativeView;

		public TNativeView NativeView { get; private set; }

		public TVirtualView VirtualView { get; private set; }

		public Type VirtualType => typeof(TVirtualView);

		public Type ComponentType => typeof(TNativeView);

		public virtual bool HasContainer { get; set; }

		public virtual SizeF GetIntrinsicSize(SizeF availableSize) => availableSize;

		public virtual void Remove(View view)
		{
		}

		public virtual void SetFrame(RectangleF frame)
		{
		}

		public virtual void SetView(View view)
		{
			VirtualView = view as TVirtualView;

			if (NativeView != null)
			{
				_mapper?.UpdateProperties(this, VirtualView);
				NativeView?.NotifyUpdate();
			}
		}

		public virtual void UpdateValue(string property, object value)
		{
			_mapper?.UpdateProperty(this, VirtualView, property);
			NativeView?.NotifyUpdate();
		}

		protected virtual void InitializeView()
		{
		}

		void IBlazorViewHandler.OnComponentLoad(object nativeView)
		{
			NativeView = (TNativeView)nativeView;
			InitializeView();

			_mapper?.UpdateProperties(this, VirtualView);
			NativeView.NotifyUpdate();
		}
	}
}
