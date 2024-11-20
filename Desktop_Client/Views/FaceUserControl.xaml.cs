using System.Windows.Controls;

namespace Desktop_Client
{
    public partial class FaceUserControl : UserControl
    {
        public FaceUserControl()
        {
            InitializeComponent();
            DataContext = new FaceVM();
        }
    }
}
