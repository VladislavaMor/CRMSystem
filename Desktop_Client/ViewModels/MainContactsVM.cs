using CRM_Helper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Desktop_Client
{
    internal class MainContactsVM : EditorVM
    {
        public MainContactsVM()
        {
            Contacts = Task.Run(async () =>
                await UserContext.SPClient.Contacts.GetAsync()).Result;
            CopyLink = new LamdaCommand(OnCopyLink, CanCopyLink);
        }

        private Contact _contacts;
        public Contact Contacts
        {
            get => _contacts;
            set
            {
                IsObjectEdit = false;
                Set(ref _contacts, value);
            }
        }
        protected virtual bool CanCopyLink(object p) => true;
        public ICommand CopyLink { get; }
        protected virtual void OnCopyLink(object p)
        {
            Clipboard.SetText(Contacts.LinkToMapContructor);
        }

        protected override void OnReturn(object p)
        {
            base.OnReturn(p);
            Contacts = Task.Run(async () =>
               await UserContext.SPClient.Contacts.GetAsync()).Result;
        }

        protected override bool CanSave(object p)
        {
            var c = Contacts;
            if (string.IsNullOrEmpty(c.Adress) || string.IsNullOrEmpty(c.PhoneNumber) || string.IsNullOrEmpty(c.Email) || string.IsNullOrEmpty(c.LinkToMapContructor))
            {
                return false;
            }
            return true;
        }
        protected override void OnSave(object p)
        {
            var c = Contacts;

            ContactsTransfer contTransfer = new()
            {
                Adress = c.Adress,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                LinkToMapContructor = c.LinkToMapContructor
            };

            Task.Run(async () => await UserContext.SPClient.Contacts.EditAsync(contTransfer));
            Contacts = Task.Run(async () =>
                await UserContext.SPClient.Contacts.GetAsync()).Result;
        }


        protected override bool CanAdd(object p)
        {
            throw new NotImplementedException();
        }
        protected override void OnDelete(object p)
        {
            throw new NotImplementedException();
        }
    }
}
