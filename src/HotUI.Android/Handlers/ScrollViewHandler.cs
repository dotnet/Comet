using Android.Content;
using AView = Android.Views.View;
using AScrollView = Android.Widget.ScrollView;

namespace HotUI.Android
{
    public class ScrollViewHandler : AbstractHandler<ScrollView,AScrollView>
    {
        public static readonly PropertyMapper<ScrollView> Mapper = new PropertyMapper<ScrollView>(ViewHandler.Mapper)
        {
            
        };
        
        private AView _content;

        public ScrollViewHandler() : base(Mapper)
        {
        }

        protected override AScrollView CreateView(Context context)
        {
            return new AScrollView(context);
        }
        
        public override void Remove(View view)
        {
            if (_content != null)
            {
                TypedNativeView.RemoveView(_content);
                _content = null;
            }
            
            base.Remove(view);
        }
        
        public override void SetView(View view)
        {
            base.SetView(view);

            var newContent = VirtualView?.View?.ToView();
            if (_content == null || newContent != _content)
            {
                if (_content != null)
                    TypedNativeView.RemoveView(_content);

                _content = newContent;

                if (_content != null)
                    TypedNativeView.AddView(_content);
            }
        }
    }
}