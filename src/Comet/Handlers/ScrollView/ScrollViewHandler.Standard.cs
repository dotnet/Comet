﻿using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
namespace Comet.Handlers
{
	public partial class ScrollViewHandler: ViewHandler<ScrollView, object>
	{
		protected override object CreatePlatformView() => throw new NotImplementedException();
		
	}
}
