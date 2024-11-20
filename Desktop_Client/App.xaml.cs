
using System.Windows;

namespace Desktop_Client
{
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var dialog = new LoginWindow();
            if (dialog.ShowDialog() == true)
            {
                var ap = ((LoginVM)dialog.DataContext).AuthParams;
                AuthData.Login = ap.Login;
                AuthData.AccessToken = ap.AccessToken;
                AuthData.IsLogin = ap.IsLogin;
                var mainWindow = new MainWindow();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                mainWindow.Show();
            }
        }
    }
}
