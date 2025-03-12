using System.Windows;

namespace OOP.Views
{
    public partial class ThicknessDialog : Window
    {
        public double SelectedThickness { get; private set; } = 2; // Значение по умолчанию

        public ThicknessDialog()
        {
            InitializeComponent();
        }

        private void StrokeThicknessSelect_Click(object sender, RoutedEventArgs e)
        {
            SelectedThickness = StrokeThicknessSlider.Value;
            DialogResult = true;
            Close();
        }
        
        private void StrokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderValueText.Text = StrokeThicknessSlider.Value.ToString("F1");
        }
    }
}