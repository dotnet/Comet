using System;

namespace System.Maui
{
	internal class MulticastAction<T>
	{
		private readonly Action<T>[] _actions;

		public MulticastAction(Binding<T> binding, Action<T> action) : this((v) => binding?.Set?.Invoke(v), action)
		{
		}

		public MulticastAction(params Action<T>[] actions)
		{
			_actions = actions;
		}

		public void Invoke(T value)
		{
			foreach (var action in _actions)
				action?.Invoke(value);
		}

		public static implicit operator Action<T>(MulticastAction<T> action) => action.Invoke;
	}
}
