using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Comet.Reflection;
using Microsoft.Maui;

namespace Comet.Tests.Handlers
{
	public class GenericViewHandler : IViewHandler
	{
		public IMauiContext MauiContext { get; private set; }
		public GenericViewHandler()
		{
		}

		public IView CurrentView { get; private set; }

		public object NativeView => throw new NotImplementedException();

		public bool HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public SizeF GetIntrinsicSize(double widthConstraint, double heightConstraint) => OnGetIntrinsicSize?.Invoke(widthConstraint,heightConstraint) ?? new Size(-1,-1);

		public void SetFrame(Rect frame)
		{
			Frame = frame;
		}

		public Func<double, double, Size> OnGetIntrinsicSize { get; set; }

		public Rect Frame
		{
			get => (Rect)ChangedProperties[nameof(Frame)];
			set => ChangedProperties[nameof(Frame)] = value;
		}

		IElement IElementHandler.VirtualView => CurrentView;
		IView IViewHandler.VirtualView => CurrentView;
		object? IElementHandler.PlatformView => CurrentView;

		public object ContainerView => throw new NotImplementedException();

		public readonly Dictionary<string, object> ChangedProperties = new Dictionary<string, object>();
		public readonly Dictionary<string, object> InvokedCommands = new Dictionary<string, object>();


		public void UpdateValue(string property)
		{
			var val = CurrentView?.GetPropValue<object>(property);
			if (val is Binding b)
				ChangedProperties[property] = b.Value;
			else
				ChangedProperties[property] = val;
		}

		public void Dispose()
		{
			ChangedProperties?.Clear();
		}

		public void SetVirtualView(IView view)
		{

			ChangedProperties.Clear();
			CurrentView = view;
		}

		void IElementHandler.SetVirtualView(IElement view) => SetVirtualView((IView)view);
		public void DisconnectHandler() => CurrentView = null;
		void IElementHandler.SetMauiContext(IMauiContext mauiContext) => MauiContext = mauiContext;
		Size IViewHandler.GetDesiredSize(double widthConstraint, double heightConstraint) => GetIntrinsicSize(widthConstraint, heightConstraint);
		void IViewHandler.PlatformArrange(Rect frame) => Frame = frame;
		public void Invoke(string command, object args = null) => InvokedCommands.Add(command, args);
	}
}
