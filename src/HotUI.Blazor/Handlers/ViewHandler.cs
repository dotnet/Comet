using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
{
    internal class ViewHandler : BlazorHandler<View, BView>
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>
        {
        };

        public ViewHandler()
            : base(Mapper)
        {
        }

        protected override void NativeViewUpdated()
        {
            base.NativeViewUpdated();

            NativeView.View = VirtualView;
        }

        public override void SetView(View view)
        {
            if (NativeView != null)
            {
                NativeView.View = view;
            }

            base.SetView(view);
        }
    }
}
