using AView = Android.Views.View;
using AScrollView = Android.Widget.ScrollView;

namespace HotUI.Android
{
    public class ScrollViewHandler : AScrollView, IView
    {
        public ScrollViewHandler() : base(AndroidContext.CurrentContext)
        {
        }
        
        public AView View => this;
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            if (content != null)
            {
                RemoveView(content);
                content = null;
            }
        }

        AView content;

        public void SetView(View view)
        {
            var scroll = view as ScrollView;

            var newContent = scroll?.View?.ToView();
            if (content == null || newContent != content)
            {
                if (content != null)
                    RemoveView(content);

                content = newContent;

                if (content != null)
                    base.AddView(content);
            }

            this.UpdateProperties(view);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }
    }
}