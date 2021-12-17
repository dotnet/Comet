using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Comet;
//Property:NewPropertyName=DefaultValue
[assembly: CometGenerate(typeof(ITextButton), nameof(ITextButton.Text), nameof(IButton.Clicked), ClassName = "Button", Skip = new[] { $"{nameof(ITextStyle.TextColor)}:{EnvironmentKeys.Colors.Color}" }, Namespace = "Comet")]
//[assembly: CometGenerate(typeof(IImageButton), nameof(IImageButton.Source), nameof(IButton.Clicked), ClassName = "ImageButton", Namespace = "Comet")]
//[assembly: CometGenerate(typeof(IBorder), BaseClass = "ContentView", Namespace = "Comet")]
[assembly: CometGenerate(typeof(IIndicatorView),nameof(IIndicatorView.Count), ClassName = "IndicatorView", Namespace = "Comet")]
[assembly: CometGenerate(typeof(IRefreshView), nameof(IRefreshView.IsRefreshing), ClassName = "RefreshView", Namespace = "Comet")]
[assembly: CometGenerate(typeof(ILabel), $"{nameof(ILabel.Text)}:Value", Namespace = "Comet", ClassName = "Text", DefaultValues = new[] { "MaxLines = 1" }, Skip = new[] { $"{nameof(ILabel.TextColor)}:{EnvironmentKeys.Colors.Color}", $"{nameof(ITextAlignment.HorizontalTextAlignment)}", $"{nameof(ITextAlignment.VerticalTextAlignment)}" })]

[assembly: CometGenerate(typeof(IEntry), nameof(IEntry.Text), nameof(IEntry.Placeholder), nameof(IEntry.Completed), ClassName = "SecureField", Skip = new[] { $"{nameof(ITextStyle.TextColor)}:{EnvironmentKeys.Colors.Color}" , $"{nameof(IEntry.IsPassword)}= true"}, DefaultValues =new[] {$"{nameof(ITextInput.MaxLength)}=-1"}, Namespace = "Comet")]
[assembly: CometGenerate(typeof(IActivityIndicator), Namespace ="Comet", Skip = new[] { $"{nameof(IActivityIndicator.IsRunning)}=true" })]
[assembly: CometGenerate(typeof(ICheckBox),  nameof(ICheckBox.IsChecked), Namespace = "Comet")]
[assembly: CometGenerate(typeof(IDatePicker), nameof(IDatePicker.Date), nameof(IDatePicker.MinimumDate), nameof(IDatePicker.MaximumDate), Namespace = "Comet")]
[assembly: CometGenerate(typeof(IProgress), $"{nameof(IProgress.Progress)}:Value", ClassName = "ProgressBar", Namespace = "Comet")]
//[assembly: CometGenerate(typeof(IRadioButton), nameof(IRadioButton.IsChecked), Namespace = "Comet")]
[assembly: CometGenerate(typeof(ISearchBar), nameof(ISearchBar.Text),$"{nameof(ISearchBar.SearchButtonPressed)}:Search", Skip = new[] { $"{nameof(ITextStyle.TextColor)}:{EnvironmentKeys.Colors.Color}" }, DefaultValues = new[] { $"{nameof(ITextInput.MaxLength)}=-1" }, Namespace = "Comet")]
[assembly: CometGenerate(typeof(IEditor), nameof(IEditor.Text), Skip = new[] { $"{nameof(ITextStyle.TextColor)}:{EnvironmentKeys.Colors.Color}" }, DefaultValues = new[] { $"{nameof(ITextInput.MaxLength)}=-1" }, ClassName="TextEditor", Namespace = "Comet")]
[assembly: CometGenerate(typeof(IEntry), nameof(IEntry.Text), nameof(IEntry.Placeholder), nameof(IEntry.Completed), Skip = new[] { $"{nameof(ITextStyle.TextColor)}:{EnvironmentKeys.Colors.Color}" }, DefaultValues = new[] { $"{nameof(ITextInput.MaxLength)}=-1" }, ClassName="TextField", Namespace = "Comet")]
[assembly: CometGenerate(typeof(ISlider), nameof(ISlider.Value),$"{nameof(ISlider.Minimum)}=0", $"{nameof(ISlider.Maximum)}=1", Namespace = "Comet")]
[assembly: CometGenerate(typeof(ISwitch), $"{nameof(ISwitch.IsOn)}:Value", ClassName="Toggle", Namespace = "Comet")]
[assembly: CometGenerate(typeof(ITimePicker), nameof(ITimePicker.Time), Namespace = "Comet")]
[assembly: CometGenerate(typeof(IStepper), nameof(IStepper.Value),nameof(IStepper.Minimum),nameof(IStepper.Maximum),nameof(IStepper.Interval), Namespace = "Comet")]
