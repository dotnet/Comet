using AppKit;
using HotUI.Mac.Controls;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Mac.Handlers
{
    public class SpacerHandler : NSColorView, MacViewHandler
    {
        public static readonly PropertyMapper<Spacer> Mapper = new PropertyMapper<Spacer>(ViewHandler.Mapper)
        {
            
        };

        private Spacer _spacer;

        public NSView View => this;

        public object NativeView => View;
        public bool HasContainer { get; set; } = false;
        public HUIContainerView ContainerView => null;

        public void Remove(View view)
        {
            _spacer = null;
        }

        public void SetView(View view)
        {
            _spacer = view as Spacer;
            Mapper.UpdateProperties(this, _spacer);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _spacer, property);
        }
    }
}