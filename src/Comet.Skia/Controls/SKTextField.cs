using System;

namespace Comet.Skia
{
	public class SKTextField : TextField
	{
		public SKTextField()
		{
		}

		public SKTextField(Binding<string> value = null, string placeholder = null, Action<string> onEditingChanged = null, Action<string> onCommit = null) : base(value, placeholder, onEditingChanged, onCommit)
		{
		}

		public SKTextField(Func<string> value, string placeholder = null, Action<string> onEditingChanged = null, Action<string> onCommit = null) : base(value, placeholder, onEditingChanged, onCommit)
		{
		}
	}
}
