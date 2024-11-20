using System.Windows.Controls;

namespace Desktop_Client
{
    internal class ContactsVM : ViewModel
    {
        public ContactsVM()
        {
            SocialNetworksUC = new SocialNetworksUserControl();
            MainContactsDataUC = new MainContactsDataUserControl();
        }

        private UserControl _socialNetworksUC;
        public UserControl SocialNetworksUC
        {
            get => _socialNetworksUC;
            set => Set(ref _socialNetworksUC, value);
        }

        private UserControl _mainContactsDataUC;
        public UserControl MainContactsDataUC
        {
            get => _mainContactsDataUC;
            set => Set(ref _mainContactsDataUC, value);
        }
    }
}
