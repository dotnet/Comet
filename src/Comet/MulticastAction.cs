using System;

namespace Comet
{
	internal class MulticastAction<T>
	{
		Action<T> action;
		Binding<T> binding;
		public MulticastAction(Binding<T> binding, Action<T> action)
		{
			this.binding = binding;
			this.action = action;
		}

		public void Invoke(T value)
		{
			binding.Set(value);
			action?.Invoke(value);
		}

		public static implicit operator Action<T>(MulticastAction<T> action) => action.Invoke;
	}
}
