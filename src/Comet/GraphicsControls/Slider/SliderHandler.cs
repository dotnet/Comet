//using Comet.Handlers;
//using Microsoft.Maui;
//using Microsoft.Maui.Graphics;

//namespace Comet.GraphicsControls
//{
//	public abstract class SliderHandler : GraphicsControlHandler<SliderHandler, ISlider>
//	{
//		bool _isDragging;

//		protected SliderHandler(DrawMapper drawMapper, PropertyMapper mapper) : base(drawMapper, mapper)
//		{

//		}

//		public override bool StartInteraction(PointF[] points)
//		{
//			_isDragging = false;
//			return base.StartInteraction(points);
//		}

//		public override void DragInteraction(PointF[] points)
//		{
//			_isDragging = true;

//			if (!_isDragging)
//				VirtualView?.DragStarted();

//			base.DragInteraction(points);
//		}

//		public override void EndInteraction(PointF[] points, bool inside)
//		{
//			_isDragging = false;

//			if (_isDragging)
//				VirtualView?.DragCompleted();

//			base.EndInteraction(points, inside);
//		}
//	}
//}