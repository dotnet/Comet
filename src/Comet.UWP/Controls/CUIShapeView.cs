using System.Drawing;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using WCanvas = Windows.UI.Xaml.Controls.Canvas;
using WPath = Windows.UI.Xaml.Shapes.Path;
using Size = Windows.Foundation.Size;

namespace Comet.UWP.Controls
{
	public class CUIShapeView : WCanvas
	{
		private Shape _shape;
		private WPath _path;
		private Size _size;

		public CUIShapeView()
		{

		}

		public Shape Shape
		{
			get => _shape;
			set
			{
				_shape = value;
				UpdateShape();
			}
		}

		public View View { get; set; }

		protected override Size ArrangeOverride(Size finalSize)
		{
			_size = base.ArrangeOverride(finalSize);
			UpdateShape();
			return _size;
		}

		private void UpdateShape()
		{
			if (_path != null)
			{
				Children.Remove(_path);
				_path = null;
			}

			if (_shape != null)
			{
				if (_size.Width <= 0 || _size.Height <= 0) return;

				var bounds = new RectangleF(0, 0, (float)_size.Width, (float)_size.Height);
				var path = _shape.PathForBounds(bounds);
				_path = new WPath()
				{
					Data = path.AsPathGeometry(),
					Stroke = new SolidColorBrush(_shape.GetStrokeColor(View, Color.Black).ToColor()),
					StrokeThickness = _shape.GetLineWidth(View, 1)
				};
				Children.Add(_path);
			}
		}
	}
}
