using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using OOP.Models;
using System.Windows;

namespace OOP.Services
{
    public class FileManager
    {
        private static List<Type> _knownShapeTypes = GetInitialShapeTypes();

        private static List<Type> GetInitialShapeTypes()
        {
            var assembly = typeof(ShapeBase).Assembly;
            var shapeBaseType = typeof(ShapeBase);
            
            return assembly.GetTypes()
                .Where(t => t != shapeBaseType && shapeBaseType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();
        }

        public static void RegisterPluginAssembly(Assembly assembly)
        {
            var shapeBaseType = typeof(ShapeBase);
            var pluginTypes = assembly.GetTypes()
                .Where(t => shapeBaseType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();
            
            _knownShapeTypes.AddRange(pluginTypes);
        }

        public void SaveShapesToFile(string filePath, List<ShapeBase> shapes)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<ShapeBase>), _knownShapeTypes.ToArray());

                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, shapes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public List<ShapeBase> LoadShapesFromFile(string filePath)
        {
            var missingShapes = new List<string>();
            var validShapes = new List<ShapeBase>();

            try
            {
                var tempFilePath = Path.GetTempFileName();
                File.Copy(filePath, tempFilePath, overwrite: true);

                try
                {
                    var xmlContent = File.ReadAllText(tempFilePath);
                    var doc = new System.Xml.XmlDocument();
                    doc.LoadXml(xmlContent);
                    
                    var nodes = doc.SelectNodes("//ShapeBase");
                    if (nodes == null || nodes.Count == 0)
                    {
                        return validShapes;
                    }
                    
                    var knownTypeNames = _knownShapeTypes.Select(t => t.Name).ToList();
                    var nodesToRemove = new List<System.Xml.XmlNode>();

                    foreach (System.Xml.XmlNode node in nodes)
                    {
                        var typeAttr = node.Attributes?["xsi:type"]?.Value;
                        if (string.IsNullOrEmpty(typeAttr)) continue;

                        var typeName = typeAttr.Contains(':') ? typeAttr.Split(':')[1] : typeAttr;
                        
                        if (!knownTypeNames.Contains(typeName))
                        {
                            missingShapes.Add(typeName);
                            nodesToRemove.Add(node);
                        }
                    }
                    
                    if (nodesToRemove.Count > 0)
                    {
                        foreach (var node in nodesToRemove)
                        {
                            node.ParentNode?.RemoveChild(node);
                        }
                        
                        doc.Save(tempFilePath);
                    }
                    
                    var serializer = new XmlSerializer(typeof(List<ShapeBase>), _knownShapeTypes.ToArray());
                    using (var reader = new StreamReader(tempFilePath))
                    {
                        if (serializer.Deserialize(reader) is List<ShapeBase> shapes)
                        {
                            validShapes = shapes.Where(shape => shape != null).ToList();
                        }
                    }
                }
                finally
                {
                    try { File.Delete(tempFilePath); }
                    catch { /* Игнорируем ошибки удаления */ }
                }

                if (missingShapes.Count > 0)
                {
                    MessageBox.Show($"Следующие типы фигур не были загружены (отсутствуют плагины):\n{string.Join("\n", missingShapes.Distinct())}", 
                                  "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return validShapes;
        }
    }
}