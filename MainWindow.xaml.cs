using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1_SEFP_program_test2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();          // Initialiserer komponenterne i MainWindow
            MainFrame.Navigate(new Side_Login()); // Start siden går til login siden
        }
    }
}