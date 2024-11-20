using System.Windows.Controls;

namespace Desktop_Client
{
    public partial class ProjectsUserControl : UserControl
    {
        public ProjectsUserControl()
        {
            InitializeComponent();
            DataContext = new ProjectsVM();
        }
    }
}
