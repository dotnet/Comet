using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Comet;

[assembly: CometGenerate(typeof(ILabel), "Text:Value", Namespace = "Comet", ClassName = "Text", DefaultValues = new[] { "MaxLines = 1" })]
[assembly: CometGenerate(typeof(IButton), "Text", nameof(IButton.Clicked), Namespace = "Comet")]


