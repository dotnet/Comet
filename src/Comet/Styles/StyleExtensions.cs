using Comet.Styles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comet
{
    public static class StyleExtensions
    {
        public static T StyleAsH1<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.H1;
            return text;
        }

        public static T StyleAsH2<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.H2;
            return text;
        }

        public static T StyleAsH3<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.H3;
            return text;
        }

        public static T StyleAsH4<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.H4;
            return text;
        }

        public static T StyleAsH5<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.H5;
            return text;
        }

        public static T StyleAsH6<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.H6;
            return text;
        }

        public static T StyleAsSubtitle1<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.Subtitle1;
            return text;
        }

        public static T StyleAsSubtitle2<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.Subtitle2;
            return text;
        }

        public static T StyleAsBody1<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.Body1;
            return text;
        }

        public static T StyleAsBody2<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.Body2;
            return text;
        }

        public static T StyleAsCaption<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.Caption;
            return text;
        }

        public static T StyleAsOverline<T>(this T text) where T : Text
        {
            text.StyleId = EnvironmentKeys.Text.Style.Overline;
            return text;
        }

        public static T ApplyStyle<T>(this T view, Style style) where T : ContextualObject
        {
            style.Apply(view);
            return view;
        }
    }
}
