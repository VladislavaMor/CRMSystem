using CRM_Helper;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Desktop_Client
{
    internal class LoginVM : ViewModel
    {
        public LoginVM()
        {
            JoinWithLogin = new LamdaCommand(OnJoinWithLogin, CanAnyWay);
            JoinAsGuest = new LamdaCommand(OnJoinAsGuest, CanAnyWay);
        }

        public AuthParameters AuthParams { get; private set; } = new();


        private string _name = "admin";
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);

        }


        private string _password = "admin";
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);

        }


        private string _error = "";
        public string Error
        {
            get => _error;
            set
            {
                Set(ref _error, value);
                OnPropertyChanged("ErrorExcist");
            }
        }

        public bool ErrorExcist
        {
            get => !(Error == string.Empty);

        }

        private bool CanAnyWay(object p) => true;
        public ICommand JoinWithLogin { get; }
        private void OnJoinWithLogin(object window)
        {
            if (window != null)
            {
                if (Name == string.Empty || Password == string.Empty)
                {
                    Error = "Поля не должны быть пустыми";
                    return;
                }

                try
                {
                    Task.Run(async () =>
                        await UserContext.SPClient.Accounts.LoginAsync(new Account() { Login = Name, Password = Password })).Wait();
                }
                catch (System.AggregateException ex) when (ex.InnerException is HttpRequestException exception)
                {
                    var sc = exception.StatusCode;
                    Error = sc switch
                    {
                        HttpStatusCode.BadRequest => "Bad format of some field",
                        HttpStatusCode.NotFound => "Name or Password is Wrong!",
                        _ => "Сonnection error",
                    };
                    return;
                }

                AuthParams.IsLogin = true;
                AuthParams.Login = Name;
                ((Window)window).DialogResult = true;
                ((Window)window).Close();
            }
        }

        public ICommand JoinAsGuest { get; }
        private void OnJoinAsGuest(object window)
        {
            if (window != null)
            {
                AuthParams = new() { IsLogin = false };
                ((Window)window).DialogResult = true;
                ((Window)window).Close();
            }
        }
    }
}
