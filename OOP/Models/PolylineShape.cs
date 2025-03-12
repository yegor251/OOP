using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace OOP.Models
{
    [Serializable]
    public class PolylineShape : ShapeBase
    {
        private List<Point> _points = new List<Point>();

        // Добавляем метод для получения точек
        public List<Point> GetPoints()
        {
            return _points;
        }

        // Добавляем метод для установки точек
        public void SetPoints(List<Point> points)
        {
            _points = points;
        }

        public void AddPoint(Point point)
        {
            _points.Add(point);
        }

        public void ClearPoints()
        {
            _points.Clear();
        }

        public override Shape Draw()
        {
            var polyline = new Polyline
            {
                Stroke = StrokeColor,
                StrokeThickness = StrokeThickness
            };

            foreach (var point in _points)
            {
                polyline.Points.Add(point);
            }

            RenderedShape = polyline;
            return polyline;
        }
        
        public override void UpdateShape(Point currentPoint)
        {
            if (RenderedShape is Polyline polyline)
            {
                polyline.Points.Add(currentPoint);
            }
        }

        public override void HandleMouseDown(Point startPoint, MouseButtonEventArgs e, Canvas canvas)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _points.Add(startPoint);
                RenderedShape = Draw();
                canvas.Children.Add(RenderedShape);
            }
        }

        public override void HandleMouseMove(Point currentPoint, Canvas canvas) {}

        public override void HandleMouseUp(Point endPoint, Canvas canvas)
        {
            _points.Add(endPoint);
            UpdateShape(endPoint);
        }
    }
}