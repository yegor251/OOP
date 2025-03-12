using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP.Models
{
    [Serializable]
    public class EllipseShape : ShapeBase
    {
        private Ellipse _tempEllipse; // Временный эллипс для отображения в процессе рисования

        public override Shape Draw()
        {
            var ellipse = new Ellipse
            {
                Stroke = StrokeColor,
                Width = Math.Abs(EndPoint.X - StartPoint.X),
                Height = Math.Abs(EndPoint.Y - StartPoint.Y),
                Fill = FillColor,
                StrokeThickness = StrokeThickness
            };

            Canvas.SetLeft(ellipse, Math.Min(StartPoint.X, EndPoint.X));
            Canvas.SetTop(ellipse, Math.Min(StartPoint.Y, EndPoint.Y));
            RenderedShape = ellipse;
            return ellipse;
        }

        public override void UpdateShape(Point currentPoint)
        {
            if (_tempEllipse != null)
            {
                _tempEllipse.Width = Math.Abs(currentPoint.X - StartPoint.X);
                _tempEllipse.Height = Math.Abs(currentPoint.Y - StartPoint.Y);
                Canvas.SetLeft(_tempEllipse, Math.Min(StartPoint.X, currentPoint.X));
                Canvas.SetTop(_tempEllipse, Math.Min(StartPoint.Y, currentPoint.Y));
            }
        }

        public override void HandleMouseDown(Point startPoint, MouseButtonEventArgs e, Canvas canvas)
        {
            StartPoint = startPoint;
            _tempEllipse = new Ellipse
            {
                Stroke = StrokeColor,
                Fill = FillColor,
                StrokeThickness = StrokeThickness
            };
            canvas.Children.Add(_tempEllipse); // Добавляем временный эллипс на холст
        }

        public override void HandleMouseMove(Point currentPoint, Canvas canvas)
        {
            if (_tempEllipse != null)
            {
                UpdateShape(currentPoint);
            }
        }

        public override void HandleMouseUp(Point endPoint, Canvas canvas)
        {
            if (_tempEllipse != null)
            {
                EndPoint = endPoint;
                UpdateShape(endPoint);

                // Удаляем временный эллипс с холста
                canvas.Children.Remove(_tempEllipse);

                // Создаем окончательный эллипс и добавляем его на холст
                RenderedShape = Draw();
                canvas.Children.Add(RenderedShape);

                // Очищаем временный эллипс
                _tempEllipse = null;
            }
        }
    }
}