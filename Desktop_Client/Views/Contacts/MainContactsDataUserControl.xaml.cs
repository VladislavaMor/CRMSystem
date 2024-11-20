using System.Windows.Controls;

namespace Desktop_Client
{

    public partial class MainContactsDataUserControl : UserControl
    {
        public MainContactsDataUserControl()
        {
            InitializeComponent();
            DataContext = new MainContactsVM();
        }
    }
}
