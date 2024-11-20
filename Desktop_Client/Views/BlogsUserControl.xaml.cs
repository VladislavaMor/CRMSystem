using System.Windows.Controls;

namespace Desktop_Client
{

    public partial class BlogsUserControl : UserControl
    {
        public BlogsUserControl()
        {
            InitializeComponent();
            DataContext = new BlogsVM();
        }
    }
}
