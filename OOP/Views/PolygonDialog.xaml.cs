using System.Windows;

namespace OOP.Views
{
    public partial class PolygonDialog : Window
    {
        public int Sides { get; private set; }

        public PolygonDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SidesTextBox.Text, out int sides) && sides > 2)
            {
                Sides = sides;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное количество сторон (больше 2).");
            }
        }
    }
}