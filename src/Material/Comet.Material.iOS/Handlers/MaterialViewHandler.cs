using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comet.iOS;
using Comet.iOS.Handlers;
using Foundation;
using MaterialComponents;
using UIKit;

namespace Comet.Material.iOS
{
    public static class MaterialViewHandler
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
            [nameof(EnvironmentKeys.View.Border)] = MapBorderProperty,
            [nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
            //[nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty,
            //[nameof(EnvironmentKeys.View.Overlay)] = MapOverlayProperty,
            [nameof(EnvironmentKeys.Animations.Animation)] = ViewHandler.MapAnimationProperty,
        };

        public static void MapBackgroundColorProperty(IViewHandler handler, View virtualView)
        {
            var materialView = handler as IMaterialView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
                materialView.ColorScheme.BackgroundColor = color.ToUIColor();

            materialView.ApplyScheme();
        }

        public static void MapBorderProperty(IViewHandler handler, View virtualView)
        {

            //TODO apply the shape.
            var materialView = handler as IMaterialView;
            //Maybe it is something to do with ShapeScheme
            //materialView.ShapeScheme.

            materialView.ApplyScheme();
        }
        public static void MapShadowProperty(IViewHandler handler, View virtualView)
        {
            //TODO: apply shadow
            var nativeView = (UIView)handler.NativeView;
            var materialView = handler as IMaterialView;

            var shadow = virtualView.GetShadow();
            var clipShape = virtualView.GetClipShape();

            materialView.ApplyScheme();
        }
    }


    public abstract class MaterialViewHandler<TVirtualView, TNativeView> : AbstractControlHandler<TVirtualView, TNativeView>,
        iOSViewHandler, IMaterialView
        where TVirtualView : View
        where TNativeView : UIView
    {
        protected MaterialViewHandler()
        {

        }

        protected MaterialViewHandler(PropertyMapper<TVirtualView> mapper) : base(mapper)
        {

        }
        private SemanticColorScheme colorScheme = new SemanticColorScheme(ColorSchemeDefaults.Material201804);
        private TypographyScheme typographyScheme = new TypographyScheme();
        private ShapeScheme shapeScheme = new ShapeScheme();

        public SemanticColorScheme ColorScheme
        {
            get => colorScheme;
            protected set
            {
                colorScheme = value;
                RecreateScheme();
            }
        }
        public TypographyScheme TypographyScheme
        {
            get => typographyScheme;
            protected set
            {
                typographyScheme = value;
                RecreateScheme();
            }
        }
        public ShapeScheme ShapeScheme
        {
            get => shapeScheme;
            protected set
            {
                shapeScheme = value;
                RecreateScheme();
            }
        }
        public abstract void ApplyScheme();

        public abstract void RecreateScheme();
    }
}