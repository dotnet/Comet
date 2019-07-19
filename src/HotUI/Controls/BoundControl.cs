using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HotUI 
{
	public abstract class BoundControl : Control
	{
		private readonly Dictionary<string, BindingDefinition> _bindings = new Dictionary<string, BindingDefinition>();
		
		protected BoundControl (params Binding[] bindings) : base(HasValueBinding(bindings)) 
		{

		}
		
		protected void Bind<T>(
			Binding<T> binding, 
			string propertyName,
			Action<object> updater)
		{
			if (binding == null) return;
			_bindings[propertyName] = new BindingDefinition(binding, propertyName, updater);
		}

		protected void AddBindings(IReadOnlyDictionary<string, BindingDefinition> bindings)
		{
			if (bindings == null) return;
			foreach (var entry in bindings)
				_bindings[entry.Key] = entry.Value;
		}
		
		protected void SetValue<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			State.SetValue (ref currentValue, newValue, this , propertyName);
		}
		
		protected override void WillUpdateView ()
		{
			base.WillUpdateView ();

			if (_bindings.Count > 0) 
			{
				foreach (var entry in _bindings)
				{
					var propertyName = entry.Key;
					var definition = entry.Value;
					State.StartProperty ();
					var value = definition.Binding.GetValue.Invoke ();
					definition.Updater(value);
					var props = State.EndProperty ();
					var propCount = props.Length;
					if (propCount > 0)
						State.BindingState.AddViewProperty(props, this, propertyName);
				}
			}
		}
		
        protected override void ViewPropertyChanged(string property, object value)
        {
	        if (_bindings.TryGetValue(property, out var definition))
		        definition.Updater(value);

            base.ViewPropertyChanged(property, value);
        }

        private static bool HasValueBinding(Binding[] bindings)
        {
	        return bindings != null && bindings.Any(b => b.IsValue);
        }
    }
	
	public class BindingDefinition
	{
		public BindingDefinition(Binding binding, string propertyName, Action<object> updater)
		{
			Binding = binding;
			PropertyName = propertyName;
			Updater = updater;
		}

		public Binding Binding { get; }
		public string PropertyName { get; }
		public Action<object> Updater {  get; }

	}
}
