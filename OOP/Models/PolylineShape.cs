using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace OOP.Models
{
    [Serializable]
    public class PolylineShape : ShapeBase
    {
        public List<Point> _points = new List<Point>();
        private Polyline _tempPolyline;
        private bool _isDrawing = false;
        private bool _isButtonPressed = false;
        private bool _firstPointSet = false;

        public List<Point> GetPoints()
        {
            return _points;
        }

        public void SetPoints(List<Point> points)
        {
            _points = points;
        }
        public override Shape Draw()
        {
            var polyline = new Polyline
            {
                Stroke = StrokeColor,
                StrokeThickness = StrokeThickness,
                Fill = FillColor
            };

            foreach (var point in _points)
            {
                polyline.Points.Add(point);
            }

            RenderedShape = polyline;
            return polyline;
        }

        public override void HandleMouseDown(Point startPoint, MouseButtonEventArgs e, Canvas canvas)
        {
            _isButtonPressed = true;

            if (!_isDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                _isDrawing = true;
                _tempPolyline = new Polyline
                {
                    Stroke = StrokeColor,
                    StrokeThickness = StrokeThickness
                };
                canvas.Children.Add(_tempPolyline);
            }

            if (e.RightButton == MouseButtonState.Pressed && _isDrawing)
            {
                HandleMouseMove(startPoint, canvas);
            }
        }

        public override void HandleMouseMove(Point currentPoint, Canvas canvas)
        {
            if (_isDrawing && _isButtonPressed && _tempPolyline != null)
            {
                var tempPoints = new PointCollection();
                
                foreach (var point in _points)
                {
                    tempPoints.Add(point);
                }
                
                if (_firstPointSet)
                {
                    tempPoints.Add(currentPoint);
                }
                
                _tempPolyline.Points = tempPoints;
            }
        }

        public override void HandleMouseUp(Point endPoint, MouseButton button, Canvas canvas)
        {
            _isButtonPressed = false;

            if (!_isDrawing) return;

            if (button == MouseButton.Right && _firstPointSet)
            {
                _points.Add(endPoint);

                if (_tempPolyline != null)
                {
                    _tempPolyline.Points = new PointCollection(_points);
                }
            }
            else if (button == MouseButton.Left)
            {
                if (!_firstPointSet)
                {
                    // Установка первой точки
                    _points.Add(endPoint);
                    _firstPointSet = true;
                    // Обновляем временную полилинию
                    if (_tempPolyline != null)
                    {
                        _tempPolyline.Points = new PointCollection(_points);
                    }
                    
                    NotifyShapeStarted();
                }
                else
                {
                    // Завершение рисования
                    _isDrawing = false;
                    _firstPointSet = false;

                    if (_tempPolyline != null)
                    {
                        canvas.Children.Remove(_tempPolyline);
                        _tempPolyline = null;
                    }
                    
                    RenderedShape = Draw();
                    canvas.Children.Add(RenderedShape);
                    NotifyShapeUpdated();
                    _points = new List<Point>();
                }
            }
        }

        public override void UpdateShape(Point currentPoint)
        {
            if (RenderedShape is Polyline polyline)
            {
                polyline.Points.Add(currentPoint);
            }
        }
    }
}