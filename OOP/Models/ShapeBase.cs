using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace OOP.Models
{
    [Serializable]
    public abstract class ShapeBase
    {
        public Point StartPoint;
        public Point EndPoint;
        
        [XmlIgnore]
        public Brush StrokeColor { get; set; } = Brushes.Black;
        [XmlIgnore]
        public Brush FillColor { get; set; } = Brushes.Transparent;
        public double StrokeThickness { get; set; } = 2;
        [XmlIgnore]
        public Shape RenderedShape { get; protected set; }
        public event Action<ShapeBase> ShapeCompleted;
        protected void NotifyShapeCompleted()
        {
            ShapeCompleted?.Invoke(this);
        }
        public event Action<ShapeBase> ShapeUpdated;
        protected void NotifyShapeUpdated()
        {
            ShapeUpdated?.Invoke(this);
        }
        public event Action<ShapeBase> ShapeStarted;
        protected void NotifyShapeStarted()
        {
            ShapeStarted?.Invoke(this);
        }
        public virtual ShapeBase DeepClone()
        {
            return (ShapeBase)this.MemberwiseClone();
        }
        public abstract Shape Draw();
        public abstract void UpdateShape(Point currentPoint);
        public abstract void HandleMouseDown(Point startPoint, MouseButtonEventArgs e, Canvas canvas);
        public abstract void HandleMouseMove(Point currentPoint, Canvas canvas);
        public abstract void HandleMouseUp(Point endPoint, MouseButton button, Canvas canvas);
        public virtual ShapeBase Create() => this;
        public void SetStart(Point start) => StartPoint = start;
        public void SetEnd(Point end) => EndPoint = end;
        [XmlElement("StrokeColor")]
        public string StrokeColorString
        {
            get => (StrokeColor as SolidColorBrush)?.Color.ToString();
            set => StrokeColor = (Brush)new BrushConverter().ConvertFromString(value);
        }
        [XmlElement("FillColor")]
        public string FillColorString
        {
            get => (FillColor as SolidColorBrush)?.Color.ToString();
            set => FillColor = (Brush)new BrushConverter().ConvertFromString(value);
        }
    }
}