using System;
using System.Collections.Generic;
using Android.Widget;
using Comet.Android.Controls;
using AView = Android.Views.View;
using AOrientation = Android.Widget.Orientation;
using System.Drawing;

namespace Comet.Android.Handlers
{
	public class VStackHandler : LinearLayout, AndroidViewHandler
	{
		public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		public VStackHandler() : base(AndroidContext.CurrentContext)
		{
			Orientation = AOrientation.Vertical;
		}

		public AView View => this;
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;
		public CUITouchGestureListener GestureListener { get; set; }
		public SizeF Measure(SizeF availableSize)
		{
			return availableSize;
		}

		public void SetFrame(RectangleF frame)
		{
			// Do nothing
		}

		public void Remove(View view)
		{
		}

		VStack stack;

		public void SetView(View view)
		{
			stack = view as VStack;
			UpdateChildren(stack);
			stack.ChildrenChanged += Stack_ChildrenChanged;
		}

		private void Stack_ChildrenChanged(object sender, EventArgs e)
		{
			UpdateChildren(stack);
		}

		public void UpdateValue(string property, object value)
		{
		}

		readonly List<AView> views = new List<AView>();

		protected void UpdateChildren(VStack stack)
		{
			var children = stack.GetChildren();
			if (views.Count == children.Count)
			{
				bool areSame = false;
				for (var i = 0; i < views.Count; i++)
				{
					var v = views[i];
					var c = children[i].ToView();
					areSame = c == v;
					if (!areSame)
					{
						break;
					}
				}

				if (areSame)
					return;
			}

			foreach (var v in views)
			{
				base.RemoveView(v);
			}

			views.Clear();
			foreach (var child in children)
			{
				var cview = child.ToView();
				if (cview != null)
				{
					views.Add(cview);
					base.AddView(cview);
				}
			}
		}
	}
}
