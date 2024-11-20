using System.Windows.Controls;

namespace Desktop_Client
{
    public partial class ServicesUserControl : UserControl
    {
        public ServicesUserControl()
        {
            InitializeComponent();
            DataContext = new ServicesVM();
        }
    }
}
