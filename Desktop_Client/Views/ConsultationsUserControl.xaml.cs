using System.Windows.Controls;

namespace Desktop_Client
{

    public partial class ConsultationsUserControl : UserControl
    {
        public ConsultationsUserControl()
        {
            InitializeComponent();
            DataContext = new AppealsVM();
        }
    }
}
