using System.Windows;

namespace Desktop_Client
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginVM();
        }

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            if(Owner != null) 
            Owner.Close();
		}
	}
}
