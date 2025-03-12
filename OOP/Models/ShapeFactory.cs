namespace OOP.Models
{
    public class ShapeFactory
    {
        public static ShapeBase CreateShape(string shapeType)
        {
            var type = Type.GetType($"OOP.Models.{shapeType}Shape");
            if (type == null || !typeof(ShapeBase).IsAssignableFrom(type))
            {
                throw new ArgumentException("Invalid shape type");
            }
            var shape = (ShapeBase)Activator.CreateInstance(type);
            
            return shape.Create();
        }
    }
}