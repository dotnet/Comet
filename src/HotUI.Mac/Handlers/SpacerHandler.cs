using AppKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Mac
{
    public class SpacerHandler : NSColorView, INSView
    {
        public static readonly PropertyMapper<Spacer, NSView, NSView> Mapper = new PropertyMapper<Spacer, NSView, NSView>(ViewHandler.Mapper)
        {
            
        };

        private Spacer _spacer;

        public NSView View => this;

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