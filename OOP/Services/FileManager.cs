using System.IO;
using System.Xml.Serialization;
using OOP.Models;

namespace OOP.Services
{
    public class FileManager
    {
        private static readonly Type[] ShapeTypes = GetShapeTypes();

        private static Type[] GetShapeTypes()
        {
            // Получаем все типы из сборки, где находится ShapeBase
            var assembly = typeof(ShapeBase).Assembly;
            var shapeBaseType = typeof(ShapeBase);
            
            // Находим все типы, которые наследуются от ShapeBase и не являются абстрактными
            return assembly.GetTypes()
                .Where(t => t != shapeBaseType && shapeBaseType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();
        }

        public void SaveShapesToFile(string filePath, List<ShapeBase> shapes)
        {
            var serializer = new XmlSerializer(typeof(List<ShapeBase>), ShapeTypes);

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, shapes);
            }
        }

        public List<ShapeBase> LoadShapesFromFile(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<ShapeBase>), ShapeTypes);

            using (var reader = new StreamReader(filePath))
            {
                return (List<ShapeBase>)serializer.Deserialize(reader);
            }
        }
    }
}