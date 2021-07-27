using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Comet;
//Property:NewPropertyName=DefaultValue
[assembly: CometGenerate(typeof(ILabel), "Text:Value", Namespace = "Comet", ClassName = "Text", DefaultValues = new[] { "MaxLines = 1" })]
[assembly: CometGenerate(typeof(IButton), nameof(IButton.Text), nameof(IButton.Clicked), Namespace = "Comet")]
[assembly: CometGenerate(typeof(IEntry), nameof(IEntry.Text), nameof(IEntry.Placeholder), nameof(IEntry.Completed), ClassName = "SecureField", Namespace = "Comet", Skip = new[] { "IsPassword=true" })]
[assembly: CometGenerate(typeof(IActivityIndicator), Namespace ="Comet", Skip = new[] { "IsRunning=true" })]
[assembly: CometGenerate(typeof(ICheckBox),  nameof(ICheckBox.IsChecked), Namespace = "Comet")]
[assembly: CometGenerate(typeof(IDatePicker), nameof(IDatePicker.Date), nameof(IDatePicker.MinimumDate), nameof(IDatePicker.MaximumDate), Namespace = "Comet")]


