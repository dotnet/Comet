using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;

namespace Comet.Android.Controls
{
	public class CometContainerView : LinearLayout
	{
		private AView _mainView;

		protected CometContainerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public CometContainerView() : base(AndroidContext.CurrentContext)
		{
		}

		public AView MainView
		{
			get => _mainView;
			set
			{
				if (_mainView != null)
				{
					RemoveView(_mainView);
				}

				_mainView = value;

				if (_mainView != null)
				{
					_mainView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
					AddView(_mainView);
				}
			}
		}
	}
}
