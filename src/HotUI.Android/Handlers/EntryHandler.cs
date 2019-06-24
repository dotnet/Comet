using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class EntryHandler : EditText, IView
    {
        public EntryHandler() : base(AndroidContext.CurrentContext)
        {
            TextChanged += HandleTextChanged;
        }
        
        private void HandleTextChanged(object sender, EventArgs e) => entry?.Completed(Text);

        public AView View => this;

        public void Remove(View view)
        {
            entry = null;
        }

        Entry entry;

        public void SetView(View view)
        {
            entry = view as Entry;
            this.UpdateProperties(entry);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this EditText view, Entry hView)
        {
            view.Text = hView?.Text;
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this EditText view, string property, object value)
        {
            switch (property)
            {
                case nameof(Entry.Text):
                    view.Text = (string) value;
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}