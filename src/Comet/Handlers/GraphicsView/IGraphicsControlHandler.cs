using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public interface IGraphicsControlHandler
	{
		Size GetDesiredSize(double widthConstraint, double heightConstraint);
		void SetFrame(Rectangle frame);
		void SetVirtualView(IView view);
		void UpdateValue(string property);
	}
}