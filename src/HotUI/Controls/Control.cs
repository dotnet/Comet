namespace HotUI
{
    public class Control : View
    {
        protected Control(bool hasConstructors) : base(hasConstructors)
        {
        }

        protected Control()
        {
        }

        protected override void RequestLayout()
        {
            // Do nothing
        }
    }
}