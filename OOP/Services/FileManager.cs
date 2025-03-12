using System.IO;
using System.Xml.Serialization;
using OOP.Models;

namespace OOP.Services
{
    public class FileManager
    {
        public void SaveShapesToFile(string filePath, List<ShapeBase> shapes)
        {
            var serializer = new XmlSerializer(typeof(List<ShapeBase>), new[] 
            { 
                typeof(LineShape), 
                typeof(RectangleShape), 
                typeof(EllipseShape), 
                typeof(PolygonShape), 
                typeof(PolylineShape) 
            });

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, shapes);
            }
        }

        public List<ShapeBase> LoadShapesFromFile(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<ShapeBase>), new[] 
            { 
                typeof(LineShape), 
                typeof(RectangleShape), 
                typeof(EllipseShape), 
                typeof(PolygonShape), 
                typeof(PolylineShape) 
            });

            using (var reader = new StreamReader(filePath))
            {
                return (List<ShapeBase>)serializer.Deserialize(reader);
            }
        }
    }
}