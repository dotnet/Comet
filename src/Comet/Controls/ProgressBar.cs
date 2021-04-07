using System;
using System.Collections.Generic;
using Microsoft.Maui;
namespace Comet
{
	public class ProgressBar : View, IProgress
	{
		protected static Dictionary<string, string> ProgressBarHandlerPropertyMapper = new()
		{
			[nameof(Value)] = nameof(IProgress.Progress),
		};

		public ProgressBar(
			Binding<double> value = null)
		{
			Value = value;
		}

		public ProgressBar(
			Func<double> value) : this((Binding<double>)value)
		{ }

		Binding<double> _value;
		public Binding<double> Value
		{
			get => _value;
			set => this.SetBindingValue(ref _value, value);
		}

		double IProgress.Progress => Value;

		protected override string GetHandlerPropertyName(string property) =>
			ProgressBarHandlerPropertyMapper.TryGetValue(property, out var value) ? value : base.GetHandlerPropertyName(property);
	}
}
