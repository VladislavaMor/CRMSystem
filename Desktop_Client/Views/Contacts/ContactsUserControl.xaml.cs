using System.Windows.Controls;

namespace Desktop_Client
{
    public partial class ContactsUserControl : UserControl
    {
        public ContactsUserControl()
        {
            InitializeComponent();
            DataContext = new ContactsVM();
        }
    }
}
