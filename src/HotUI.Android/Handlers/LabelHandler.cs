using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class LabelHandler : TextView, IViewHandler, IView
    {
        public AView View => this;

        public LabelHandler() : base(AndroidContext.CurrentContext)
        {
            
        }

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            var label = view as Label;
            this.UpdateLabelProperties(label);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateLabelProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateLabelProperties(this TextView view, Label hView)
        {
            view.Text = hView?.Text;
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateLabelProperty(this TextView view, string property, object value)
        {
            switch (property)
            {
                case nameof(Label.Text):
                    view.Text = (string) value;
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}