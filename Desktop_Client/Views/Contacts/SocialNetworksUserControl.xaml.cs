using System.Windows.Controls;

namespace Desktop_Client
{
    public partial class SocialNetworksUserControl : UserControl
    {
        public SocialNetworksUserControl()
        {
            InitializeComponent();
            DataContext = new SocialNetworksVM();
        }
    }
}
