using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace OOP.Models
{
    [Serializable]
    public class LineShape : ShapeBase
    {
        private Line _tempLine;

        public override Shape Draw()
        {
            var line = new Line
            {
                X1 = StartPoint.X,
                Y1 = StartPoint.Y,
                X2 = EndPoint.X,
                Y2 = EndPoint.Y,
                Stroke = StrokeColor,
                StrokeThickness = StrokeThickness
            };
            RenderedShape = line;
            return line;
        }

        public override void UpdateShape(Point currentPoint)
        {
            if (_tempLine != null)
            {
                _tempLine.X2 = currentPoint.X;
                _tempLine.Y2 = currentPoint.Y;
            }
        }

        public override void HandleMouseDown(Point startPoint, MouseButtonEventArgs e, Canvas canvas)
        {
            StartPoint = startPoint;
            _tempLine = new Line
            {
                X1 = StartPoint.X,
                Y1 = StartPoint.Y,
                Stroke = StrokeColor,
                StrokeThickness = StrokeThickness
            };
            canvas.Children.Add(_tempLine);
        }

        public override void HandleMouseMove(Point currentPoint, Canvas canvas)
        {
            if (_tempLine != null)
            {
                UpdateShape(currentPoint);
            }
        }

        public override void HandleMouseUp(Point endPoint, Canvas canvas)
        {
            if (_tempLine != null)
            {
                EndPoint = endPoint;
                UpdateShape(endPoint);

                canvas.Children.Remove(_tempLine);

                RenderedShape = Draw();
                canvas.Children.Add(RenderedShape);

                _tempLine = null;
            }
        }
    }
}