using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MaterialComponents;
using UIKit;

namespace Comet.Material.iOS
{
    public interface IMaterialView
    {
        SemanticColorScheme ColorScheme { get;}

        TypographyScheme TypographyScheme { get;}

        ShapeScheme ShapeScheme { get; }
        void ApplyScheme();
        void RecreateScheme();
    }   
}