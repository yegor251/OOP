using System.Collections.Generic;
using System.Windows;

namespace OOP.Models
{
    public class ShapeData
    {
        public string Type { get; set; } // Тип фигуры (LineShape, RectangleShape и т.д.)
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public string StrokeColor { get; set; } // Hex-код цвета обводки
        public string FillColor { get; set; }   // Hex-код цвета заливки
        public double StrokeThickness { get; set; }
        public List<Point> Points { get; set; } // Список точек для ломаной линии
        public int Sides { get; set; } // Количество сторон для многоугольника
    }
}