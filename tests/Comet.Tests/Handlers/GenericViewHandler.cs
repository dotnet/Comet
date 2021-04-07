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

		public SizeF GetIntrinsicSize(Size availableSize) => OnGetIntrinsicSize?.Invoke(availableSize) ?? View.UseAvailableWidthAndHeight;

		public void SetFrame(RectangleF frame)
		{
			Frame = frame;
		}

		public Func<Size, Size> OnGetIntrinsicSize { get; set; }

		public Rectangle Frame
		{
			get => (Rectangle)ChangedProperties[nameof(Frame)];
			set => ChangedProperties[nameof(Frame)] = value;
		}

		public readonly Dictionary<string, object> ChangedProperties = new Dictionary<string, object>();


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
		public void DisconnectHandler() => CurrentView = null;
		void IViewHandler.SetMauiContext(IMauiContext mauiContext) => MauiContext = mauiContext;
		Size IViewHandler.GetDesiredSize(double widthConstraint, double heightConstraint) => OnGetIntrinsicSize?.Invoke(new Size(widthConstraint, heightConstraint)) ?? View.UseAvailableWidthAndHeight;
		void IViewHandler.SetFrame(Rectangle frame) => Frame = frame;
	}
}
