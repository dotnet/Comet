using System;
using System.Collections.Generic;
using System.Graphics;
using Comet.Reflection;

namespace Comet.Tests.Handlers
{
	public class GenericViewHandler : IViewHandler
	{
		public GenericViewHandler()
		{
		}

		public View CurrentView { get; private set; }

		public object NativeView => throw new NotImplementedException();

		public bool HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		
		public SizeF GetIntrinsicSize(SizeF availableSize) => OnGetIntrinsicSize?.Invoke(availableSize) ?? View.UseAvailableWidthAndHeight;

		public void SetFrame(RectangleF frame)
		{
			Frame = frame;
		}

		public Func<SizeF, SizeF> OnGetIntrinsicSize { get; set; }

		public RectangleF Frame
		{
			get => (RectangleF)ChangedProperties[nameof(Frame)];
			set => ChangedProperties[nameof(Frame)] = value;
		}

		public readonly Dictionary<string, object> ChangedProperties = new Dictionary<string, object>();
		public void Remove(View view)
		{
			CurrentView = null;
		}

		public void SetView(View view)
		{
			ChangedProperties.Clear();
			CurrentView = view;
		}

		public void UpdateValue(string property, object value)
		{
			ChangedProperties[property] = value;

			var val = CurrentView.GetPropertyValue(property) as Binding;

			//TODO: This may break things
			//if (val != null)
			//    val.GetValue();


		}

		public void Dispose()
		{
			ChangedProperties?.Clear();
		}
	}
}
