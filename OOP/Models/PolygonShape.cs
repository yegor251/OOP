using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OOP.Views;

namespace OOP.Models
{
    [Serializable]
    public class PolygonShape : ShapeBase
    {
        private Polygon _tempPolygon;
        public int Sides { get; set; } = 3;

        public override Shape Draw()
        {
            var polygon = new Polygon
            {
                Stroke = StrokeColor,
                Fill = FillColor,
                StrokeThickness = StrokeThickness
            };

            PointCollection points = new PointCollection();
            double centerX = (StartPoint.X + EndPoint.X) / 2;
            double centerY = (StartPoint.Y + EndPoint.Y) / 2;
            double radiusX = Math.Abs(EndPoint.X - StartPoint.X) / 2;
            double radiusY = Math.Abs(EndPoint.Y - StartPoint.Y) / 2;

            for (int i = 0; i < Sides; i++)
            {
                double angle = 2 * Math.PI * i / Sides;
                double x = centerX + radiusX * Math.Cos(angle);
                double y = centerY + radiusY * Math.Sin(angle);
                points.Add(new Point(x, y));
            }

            polygon.Points = points;
            RenderedShape = polygon;
            return polygon;
        }

        public override void UpdateShape(Point currentPoint)
        {
            if (_tempPolygon != null)
            {
                PointCollection points = new PointCollection();
                double centerX = (StartPoint.X + currentPoint.X) / 2;
                double centerY = (StartPoint.Y + currentPoint.Y) / 2;
                double radiusX = Math.Abs(currentPoint.X - StartPoint.X) / 2;
                double radiusY = Math.Abs(currentPoint.Y - StartPoint.Y) / 2;

                for (int i = 0; i < Sides; i++)
                {
                    double angle = 2 * Math.PI * i / Sides;
                    double x = centerX + radiusX * Math.Cos(angle);
                    double y = centerY + radiusY * Math.Sin(angle);
                    points.Add(new Point(x, y));
                }

                _tempPolygon.Points = points;
            }
        }

        public override void HandleMouseDown(Point startPoint, MouseButtonEventArgs e, Canvas canvas)
        {
            StartPoint = startPoint;
            _tempPolygon = new Polygon
            {
                Stroke = StrokeColor,
                Fill = FillColor,
                StrokeThickness = StrokeThickness
            };
            canvas.Children.Add(_tempPolygon);
        }

        public override void HandleMouseMove(Point currentPoint, Canvas canvas)
        {
            if (_tempPolygon != null)
            {
                UpdateShape(currentPoint);
            }
        }

        public override void HandleMouseUp(Point endPoint, Canvas canvas)
        {
            if (_tempPolygon != null)
            {
                EndPoint = endPoint;
                UpdateShape(endPoint);

                canvas.Children.Remove(_tempPolygon);

                RenderedShape = Draw();
                canvas.Children.Add(RenderedShape);

                _tempPolygon = null;
            }
        }

        public override ShapeBase Create()
        {
            var dialog = new PolygonDialog();
            if (dialog.ShowDialog() == true)
            {
                return new PolygonShape { Sides = dialog.Sides };
            }
            return null;
        }
    }
}