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
            foreach (var child in ButtonsPanel.Children)
            {
                if (child is Button button)
                {
                    button.Style = (Style)FindResource("BaseButtonStyle");
                }
            }
            if (sender is Button selectedButton)
            {
                selectedButton.Style = (Style)FindResource("GreenButtonStyle");
            }

            if (sender is Button button1)
            {
                string shapeType = button1.Name.Replace("Button", "");
                
                var shape = ShapeFactory.CreateShape(shapeType);
                
                if (shape != null)
                {
                    _drawingManager.StartDrawing(shape);
                }
            }
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
            _drawingManager.HandleMouseUp(e.GetPosition(DrawingCanvas));
            UpdateUndoRedoButtons();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            var shape = _historyManager.Undo();
            if (shape != null && shape.RenderedShape != null)
            {
                DrawingCanvas.Children.Remove(shape.RenderedShape);
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
                DrawingCanvas.Children.Clear();

                foreach (var shape in shapes)
                {
                    DrawingCanvas.Children.Add(shape.Draw());
                    _historyManager.AddShape(shape);
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
    }
}