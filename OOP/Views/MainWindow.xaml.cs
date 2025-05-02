using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using OOP.Models;
using OOP.Services;

namespace OOP.Views
{
    public partial class MainWindow : Window
    {
        private DrawingManager _drawingManager = new DrawingManager();
        private DrawingHistoryManager _historyManager = new DrawingHistoryManager();

        public MainWindow()
        {
            InitializeComponent();
            _drawingManager.DrawingCanvas = DrawingCanvas;
            _drawingManager.HistoryManager = _historyManager;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем стили кнопок в ButtonsPanel
            foreach (var child in ButtonsPanel.Children)
            {
                if (child is Button button)
                {
                    button.Style = (Style)FindResource("BaseButtonStyle");
                }
            }
    
            // Сбрасываем стили кнопок в ShapesPanel
            foreach (var child in ShapesPanel.Children)
            {
                if (child is Button button)
                {
                    button.Style = (Style)FindResource("ShapesMenuButtonStyle");
                }
            }
    
            // Устанавливаем зеленый стиль для выбранной кнопки
            if (sender is Button selectedButton)
            {
                selectedButton.Style = (Style)FindResource("GreenButtonStyle");
            }

            if (sender is Button button1)
            {
                string shapeType = button1.Name.Replace("Button", "");
                
                if (button1.Tag is Type shapeTypeFromTag)
                {
                    var shape = (ShapeBase)Activator.CreateInstance(shapeTypeFromTag);
                    var createdShape = shape.Create();
                    if (createdShape != null)
                    {
                        _drawingManager.StartDrawing(createdShape);
                    }
                }
                else
                {
                    var shape = ShapeFactory.CreateShape(shapeType);
                    if (shape != null)
                    {
                        _drawingManager.StartDrawing(shape);
                    }
                }
            }
            ShapesMenuItem.IsSubmenuOpen = false;
        }

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _drawingManager.HandleMouseDown(e.GetPosition(DrawingCanvas), e);
            UpdateUndoRedoButtons();
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            _drawingManager.HandleMouseMove(e.GetPosition(DrawingCanvas));
        }

        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _drawingManager.HandleMouseUp(e.GetPosition(DrawingCanvas), e.ChangedButton);
            UpdateUndoRedoButtons();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            _historyManager.Undo();
            if (DrawingCanvas.Children.Count > 0)
            {
                DrawingCanvas.Children.RemoveAt(DrawingCanvas.Children.Count - 1);
                UpdateUndoRedoButtons();
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            var shape = _historyManager.Redo();
            if (shape != null)
            {
                DrawingCanvas.Children.Add(shape.Draw());
                UpdateUndoRedoButtons();
            }
        }

        private void UpdateUndoRedoButtons()
        {
            UndoButton.IsEnabled = _historyManager.CanUndo;
            RedoButton.IsEnabled = _historyManager.CanRedo;
        }

        private void StrokeColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerDialog picker = new ColorPickerDialog();
            Color? selectedColor = picker.ShowDialog();
            if (selectedColor.HasValue)
            {
                _drawingManager.SetStrokeColor(new SolidColorBrush(selectedColor.Value));
            }
        }

        private void FillColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerDialog picker = new ColorPickerDialog();
            Color? selectedColor = picker.ShowDialog();
            if (selectedColor.HasValue)
            {
                _drawingManager.SetFillColor(new SolidColorBrush(selectedColor.Value));
            }
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "XML файл (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                var fileManager = new FileManager();
                var shapes = _historyManager.GetShapes();
                fileManager.SaveShapesToFile(saveFileDialog.FileName, shapes);
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "XML файл (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileManager = new FileManager();
                var shapes = fileManager.LoadShapesFromFile(openFileDialog.FileName);
                
                _historyManager.Clear();
                UpdateUndoRedoButtons();
                DrawingCanvas.Children.Clear();

                foreach (var shape in shapes)
                {
                    DrawingCanvas.Children.Add(shape.Draw());
                }
            }
        }
        
        private void StrokeThicknessButton_Click(object sender, RoutedEventArgs e)
        {
            var thicknessDialog = new ThicknessDialog();
            if (thicknessDialog.ShowDialog() == true)
            {
                _drawingManager.SetStrokeThickness(thicknessDialog.SelectedThickness);
            }
        }
        
        private void AddShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "DLL files (*.dll)|*.dll",
                Title = "Выберите DLL с фигурой"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var assembly = System.Reflection.Assembly.LoadFrom(openFileDialog.FileName);
                    FileManager.RegisterPluginAssembly(assembly);
                    var shapeTypes = assembly.GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(ShapeBase)) && !t.IsAbstract);
                    Console.WriteLine(assembly);

                    foreach (var type in shapeTypes)
                    {
                        var button = new Button
                        {
                            Content = type.Name.Replace("Shape", ""),
                            Name = type.Name.Replace("Shape", "Button"),
                            Style = (Style)FindResource("ShapesMenuButtonStyle"),
                            Tag = type
                        };
                        button.Click += Button_Click;
                        ShapesPanel.Children.Insert(ShapesPanel.Children.Count - 1, button);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки фигуры: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            ShapesMenuItem.IsSubmenuOpen = false;
        }
    }
}