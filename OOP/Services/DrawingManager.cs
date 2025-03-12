using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using OOP.Models;
using Point = System.Windows.Point;
using OOP.Services;

public class DrawingManager
{
    private ShapeBase _currentShape;
    private bool _isDrawing;
    private Brush _strokeColor = Brushes.Black;
    private Brush _fillColor = Brushes.Transparent;
    private double _strokeThickness = 2;

    public Canvas DrawingCanvas { get; set; }
    public DrawingHistoryManager HistoryManager { get; set; }

    public void StartDrawing(ShapeBase shape)
    {
        _currentShape = shape;
    }

    public void HandleMouseDown(Point startPoint, MouseButtonEventArgs e)
    {
        if (_currentShape == null) return;
        _isDrawing = true;
        _currentShape.StrokeColor = _strokeColor;
        _currentShape.FillColor = _fillColor;
        _currentShape.StrokeThickness = _strokeThickness;
        _currentShape.HandleMouseDown(startPoint, e, DrawingCanvas);
    }

    public void HandleMouseMove(Point currentPoint)
    {
        if (_currentShape == null || !_isDrawing) return;
        _currentShape.HandleMouseMove(currentPoint, DrawingCanvas);
    }

    public void HandleMouseUp(Point endPoint)
    {
        if (_currentShape == null || !_isDrawing) return;
        _isDrawing = false;
        _currentShape.HandleMouseUp(endPoint, DrawingCanvas);
        HistoryManager.AddShape(_currentShape);
    }

    public void SetStrokeColor(Brush color)
    {
        _strokeColor = color;
    }

    public void SetFillColor(Brush color)
    {
        _fillColor = color;
    }

    public void SetStrokeThickness(double thickness)
    {
        _strokeThickness = thickness;
    }
}