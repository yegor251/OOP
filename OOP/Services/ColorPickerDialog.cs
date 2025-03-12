using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using WindowStartupLocation = System.Windows.WindowStartupLocation;

public class ColorPickerDialog
{
    public Color? ShowDialog()
    {
        var colorDialog = new ColorCanvas();
        var okButton = new Button { Content = "Выбрать", Width = 80, Margin = new Thickness(5) };

        var mainPanel = new StackPanel();
        mainPanel.Children.Add(colorDialog);
        mainPanel.Children.Add(okButton);

        var window = new Window
        {
            Title = "Выберите цвет",
            Content = mainPanel,
            Width = 300,
            Height = 400,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        Color? selectedColor = null;

        okButton.Click += (s, args) =>
        {
            selectedColor = colorDialog.SelectedColor;
            window.DialogResult = true;
            window.Close();
        };

        window.ShowDialog();

        return selectedColor;
    }
}