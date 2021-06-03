using System;
using System.Linq;
using CoreGraphics;
using CoreText;
using UIKit;
using Comet.Internal;
namespace Comet.iOS
{
	public static partial class iOSExtensions
	{


		static iOSExtensions()
		{
			Comet.iOS.UI.Init();
		}
		public static UIViewController ToViewController(this View view, bool allowNav = true)
		{
			var vc = new CometViewController
			{
				CurrentView = view,
			};

			if (vc.View != null)
			{
				if ((view.BuiltView ?? view) is NavigationView nav && allowNav)
				{
					var navController = new CUINavigationController();
					nav.SetPerformNavigate((toView) => {
						//Since iOS doesn't allow nested navigations, pass the navigate along
						if (toView is NavigationView newNav)
						{
							newNav.SetPerformNavigate(nav);
							newNav.SetPerformPop(nav);
						}

						toView.Navigation = nav;
						navController.PushViewController(toView.ToViewController(false), true);
					});
					nav.SetPerformPop(() => navController.PopViewController(true));
					navController.PushViewController(vc, false);
					return navController;
				}
			}
			return vc;
		}
		public static iOSViewHandler GetOrCreateViewHandler(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null)
			{
				var builtView = view.GetView();
				handler = Registrar.Handlers.GetHandler(builtView.GetType()) as IViewHandler;
				builtView.ViewHandler = handler;
			}
			var iUIView = handler as iOSViewHandler;
			return iUIView;
		}
		public static UIView ToView(this View view)
		{
			var handler = view.GetOrCreateViewHandler();
			return handler?.View;
		}

		public static Color ToColor(this UIColor color)
		{
			if (color == null)
				return null;

			color.GetRGBA(out var red, out var green, out var blue, out var alpha);
			return new Color((float)red, (float)green, (float)blue, (float)alpha);
		}

		public static UIColor ToUIColor(this Color color)
		{
			if (color == null)
				return null;

			return new UIColor(color.R, color.G, color.B, color.A);
		}

		public static CGColor ToCGColor(this Color color)
		{
			if (color == null)
				return null;

			return new CGColor(color.R, color.G, color.B, color.A);
		}

		// UIFontWeight[Constant] is internal in Xamarin.iOS but the convertion from
		// the public (int-based) enum is not helpful in this case.
		// -1.0 (Thin / 100) to 1.0 (Black / 900) with 0 being Regular (400)
		// which is not quite the center, not are the constant values linear
		static readonly (float value, Weight weight)[] map = new(float, Weight)[] {
			(-0.80f, Weight.Ultralight),
			(-0.60f, Weight.Thin),
			(-0.40f, Weight.Light),
			(0.0f, Weight.Regular),
			(0.23f, Weight.Medium),
			(0.30f, Weight.Semibold),
			(0.40f, Weight.Bold),
			(0.56f, Weight.Heavy),
			(0.62f, Weight.Black)
		};

		static Weight ToClosestWeight(this float? self)
		{
			foreach (var (value, weight) in map)
			{
				if (value <= self)
					return weight;
			}
			return Weight.Black;
		}

		public static FontAttributes ToFont(this UIFont font)
		{
			if (font == null)
			{
				return new FontAttributes()
				{
					Family = Device.FontService.SystemFontName,
					Size = (float)UIFont.SystemFontSize,
					Weight = Weight.Regular,
					Italic = false
				};
			}
			else
			{
				var fat = font.FontDescriptor.FontAttributes.Traits;
				return new FontAttributes
				{
					Family = font.Name,
					Size = (float)font.PointSize,
					Weight = fat == null ? Weight.Regular : fat.Weight.ToClosestWeight(),
					Italic = fat == null ? false : fat.Slant >= 30.0f,
				};
			}
		}

		static float ToConstant (this Weight self)
		{
			foreach (var (value, weight) in map)
			{
				if (self <= weight)
					return value;
			}
			return 1.0f;
		}

		public static UIFont ToUIFont(this FontAttributes attributes)
		{
			if (attributes == null)
				return UIFont.SystemFontOfSize(UIFont.SystemFontSize);

			UIFont font = (attributes.Family == Device.FontService.SystemFontName)
				? UIFont.SystemFontOfSize(attributes.Size)
				: UIFont.FromName(attributes.Family, attributes.Size);

			// the above gets us a regular, non italic font - which might be just what we need
			if (!attributes.Italic && attributes.Weight == Weight.Regular)
				return font;

			var a = new UIFontAttributes()
			{
				Traits = new UIFontTraits()
				{
					Weight = attributes.Weight.ToConstant(),
					Slant = attributes.Italic ? 30.0f : 0.0f
				}
			};

			using (font)
			{
				using var d = font.FontDescriptor.CreateWithAttributes(a);
				return UIFont.FromDescriptor(d, attributes.Size);
			}
		}

		public static UIViewController GetViewController(this UIView view)
		{
			if (view.NextResponder is UIViewController vc)
				return vc;
			if (view.NextResponder is UIView uIView)
				return uIView.GetViewController();
			return null;
		}


		public static UIGestureRecognizer ToGestureRecognizer(this Gesture gesture)
		{
			if (gesture is TapGesture tapGesture)
			{
				return new CUITapGesture(tapGesture);
			}
			throw new NotImplementedException();
		}

		public static UILineBreakMode ToUILineBreakMode(this LineBreakMode mode)
		{
			switch (mode)
			{
				case LineBreakMode.CharacterWrap:
					return UILineBreakMode.CharacterWrap;
				case LineBreakMode.HeadTruncation:
					return UILineBreakMode.HeadTruncation;
				case LineBreakMode.MiddleTruncation:
					return UILineBreakMode.MiddleTruncation;
				case LineBreakMode.TailTruncation:
					return UILineBreakMode.TailTruncation;
				case LineBreakMode.WordWrap:
					return UILineBreakMode.WordWrap;
				case LineBreakMode.NoWrap:
				default:
					return UILineBreakMode.Clip;
			}
		}
	}
}
