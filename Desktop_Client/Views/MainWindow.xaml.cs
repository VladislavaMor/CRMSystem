using System.Windows;

namespace Desktop_Client
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();
        }
    }
}
