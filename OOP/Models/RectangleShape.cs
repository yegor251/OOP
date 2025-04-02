using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP.Models
{
    [Serializable]
    public class RectangleShape : ShapeBase
    {
        private Rectangle _tempRectangle;

        public override Shape Draw()
        {
            var rectangle = new Rectangle
            {
                Stroke = StrokeColor,
                Width = Math.Abs(EndPoint.X - StartPoint.X),
                Height = Math.Abs(EndPoint.Y - StartPoint.Y),
                Fill = FillColor,
                StrokeThickness = StrokeThickness
            };

            Canvas.SetLeft(rectangle, Math.Min(StartPoint.X, EndPoint.X));
            Canvas.SetTop(rectangle, Math.Min(StartPoint.Y, EndPoint.Y));
            RenderedShape = rectangle;
            return rectangle;
        }

        public override void UpdateShape(Point currentPoint)
        {
            if (_tempRectangle != null)
            {
                _tempRectangle.Width = Math.Abs(currentPoint.X - StartPoint.X);
                _tempRectangle.Height = Math.Abs(currentPoint.Y - StartPoint.Y);
                Canvas.SetLeft(_tempRectangle, Math.Min(StartPoint.X, currentPoint.X));
                Canvas.SetTop(_tempRectangle, Math.Min(StartPoint.Y, currentPoint.Y));
            }
        }

        public override void HandleMouseDown(Point startPoint, MouseButtonEventArgs e, Canvas canvas)
        {
            StartPoint = startPoint;
            _tempRectangle = new Rectangle
            {
                Stroke = StrokeColor,
                Fill = FillColor,
                StrokeThickness = StrokeThickness
            };
            canvas.Children.Add(_tempRectangle);
        }

        public override void HandleMouseMove(Point currentPoint, Canvas canvas)
        {
            if (_tempRectangle != null)
            {
                UpdateShape(currentPoint);
            }
        }

        public override void HandleMouseUp(Point endPoint, MouseButton button, Canvas canvas)
        {
            if (_tempRectangle != null)
            {
                EndPoint = endPoint;
                UpdateShape(endPoint);
                
                canvas.Children.Remove(_tempRectangle);
                
                RenderedShape = Draw();
                canvas.Children.Add(RenderedShape);
                NotifyShapeCompleted();
                
                _tempRectangle = null;
            }
        }
    }
}