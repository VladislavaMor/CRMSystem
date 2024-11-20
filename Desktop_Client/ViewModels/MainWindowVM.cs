
using System.Windows.Controls;
using System.Windows.Input;

namespace Desktop_Client
{
    internal class MainWindowVM : ViewModel
    {
        public MainWindowVM()
        {
            OpenConsultationUC = new LamdaCommand(OnOpenConsultationUC, CanOpenConsultationUC);
            OpenProjectsUC = new LamdaCommand(OnOpenProjectsUC, CanOpenProjectsUC);
            OpenServicesUC = new LamdaCommand(OnOpenServicesUC, CanOpenServicesUC);
            OpenBlogsUC = new LamdaCommand(OnOpenBlogsUC, CanOpenBlogsUC);
            OpenContactsUC = new LamdaCommand(OnOpenContactsUC, CanOpenContactsUC);
            OpenFaceUC = new LamdaCommand(OnOpenFaceUC, CanOpenFaceUC);
        }

        private UserControl _page;
        public UserControl Page
        {
            get => _page;
            set => Set(ref _page, value);
        }

        private bool CanOpenConsultationUC(object p) => !(Page != null && Page is ConsultationsUserControl);
        public ICommand OpenConsultationUC { get; }
        private void OnOpenConsultationUC(object p)
        {
            Page = new ConsultationsUserControl();
        }

        private bool CanOpenFaceUC(object p) => !(Page != null && Page is FaceUserControl);
        public ICommand OpenFaceUC { get; }
        private void OnOpenFaceUC(object p)
        {
            Page = new FaceUserControl();
        }

        private bool CanOpenProjectsUC(object p) => !(Page != null && Page is ProjectsUserControl);
        public ICommand OpenProjectsUC { get; }
        private void OnOpenProjectsUC(object p)
        {
            Page = new ProjectsUserControl();
        }

        private bool CanOpenServicesUC(object p) => !(Page != null && Page is ServicesUserControl);
        public ICommand OpenServicesUC { get; }
        private void OnOpenServicesUC(object p)
        {
            Page = new ServicesUserControl();
        }

        private bool CanOpenBlogsUC(object p) => !(Page != null && Page is BlogsUserControl);
        public ICommand OpenBlogsUC { get; }
        private void OnOpenBlogsUC(object p)
        {
            Page = new BlogsUserControl();
        }

        private bool CanOpenContactsUC(object p) => !(Page != null && Page is ContactsUserControl);
        public ICommand OpenContactsUC { get; }
        private void OnOpenContactsUC(object p)
        {
            Page = new ContactsUserControl();
        }
    }
}
