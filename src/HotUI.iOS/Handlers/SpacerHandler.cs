using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class SpacerHandler : UIView, IUIView
    {
        private static readonly PropertyMapper<Spacer, UIView> Mapper = new PropertyMapper<Spacer, UIView>()
        {
            
        };

        private Spacer _spacer;

        public UIView View => this;

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