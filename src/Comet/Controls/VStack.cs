	using Comet.Layout;

	namespace Comet 
{
	public class VStack : AbstractLayout 
	{
        public VStack(
            HorizontalAlignment alignment = HorizontalAlignment.Center,
            float? spacing = null) : base(new VStackLayoutManager(alignment, spacing))
        {

        }
	}
}
