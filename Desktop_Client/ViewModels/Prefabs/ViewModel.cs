using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Desktop_Client
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public bool IsLogin { get => AuthData.IsLogin; }
        public string? AccessToken { get => AuthData.AccessToken; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T fieled, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(fieled, value)) return false;

            fieled = value;

            OnPropertyChanged(PropertyName);

            return true;

        }
    }
}
