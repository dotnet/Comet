namespace HotUI
{
    public class Control : View
    {
        public Control(bool hasConstructors) : base(hasConstructors)
        {
        }

        public Control()
        {
        }
        
        public override void LayoutSubviews(RectangleF bounds)
        {
            // Do nothing
        }
    }
}