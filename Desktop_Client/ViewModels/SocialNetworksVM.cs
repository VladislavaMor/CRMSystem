using CRM_Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Desktop_Client
{
    internal class SocialNetworksVM : EditorVM
    {
        public SocialNetworksVM()
        {
            SocialNetworks = new(GetSocialNetworks());
            SelectImage = new LamdaCommand(OnSelectImage, CanSelectImage);
        }

        private List<SocialMedia> GetSocialNetworks() =>
            Task.Run(async () => await UserContext.SPClient.SocialNetworks.GetListAsync()).Result;

        protected override bool CanAdd(object p) => base.CanAdd(p);
        protected override void OnAdd(object p)
        {
            base.OnAdd(p);

            Link = "Link";
            ImageName = null;
        }


        protected override bool CanDelete(object p) => base.CanDelete(p);
        protected override void OnDelete(object p)
        {
            Task.Run(async () => await UserContext.SPClient.SocialNetworks.DeleteByIdAsync(SelectedSocialNetwork!.Id.ToString())).Wait();

            SocialNetworks = new(GetSocialNetworks());
            SelectedSocialNetwork = null;
            IsObjectSelect = false;
            _lastSelectedSocialNetworkId = Guid.Empty;

        }

        protected override bool CanReturn(object p) => base.CanReturn(p);
        protected override void OnReturn(object p)
        {
            base.OnReturn(p);

            if (!IsAddObject)
            {
                SocialNetworks = new(GetSocialNetworks());
                if (_lastSelectedSocialNetworkId != Guid.Empty)
                {
                    SelectedSocialNetwork = SocialNetworks.First(p => p.Id == _lastSelectedSocialNetworkId);
                }

            }
        }


        protected override bool CanSave(object p)
        {
            if (IsAddObject && File.Exists(ImageName) && Link != string.Empty)
                return true;

            else if (!IsAddObject && Link != string.Empty)
                return true;

            return false;
        }

        protected override void OnSave(object p)
        {
            FileStream? fstream = File.Exists(ImageName) ? File.OpenRead(ImageName) : null;
            SocialMediaTransfer newSocialNetwork = new()
            {
                Link = Link,
            };

            if (IsAddObject)
            {
                Task.Run(async () => await UserContext.SPClient.SocialNetworks.AddAsync(newSocialNetwork, fstream)).Wait();
                SocialNetworks = new(GetSocialNetworks());

            }
            else
            {
                Task.Run(async () =>
                await UserContext.SPClient.SocialNetworks
                    .EditAsync(SelectedSocialNetwork.Id.ToString(), newSocialNetwork, fstream)).Wait();
                SocialNetworks = new(GetSocialNetworks());
            }
            SelectedSocialNetwork = null;
            IsObjectEdit = false;
        }

        private bool CanSelectImage(object p) => true;
        public ICommand SelectImage { get; }
        private void OnSelectImage(object p)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image Files (*.png, *.jpg)|*.png;*.jpg";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                ImageName = openFileDialog.FileName;

        }

        private string _link = "";
        public string Link
        {
            get => _link;
            set => Set(ref _link, value);

        }

        private string _imageName = "";
        public string ImageName
        {
            get => _imageName;
            set => Set(ref _imageName, value);

        }

        private Guid _lastSelectedSocialNetworkId;

        private ObservableCollection<SocialMedia> _socialNetworks;
        public ObservableCollection<SocialMedia> SocialNetworks
        {
            get => _socialNetworks;
            set => Set(ref _socialNetworks, value);
        }

        private SocialMedia? _selectedSocialNetwork;
        public SocialMedia? SelectedSocialNetwork
        {
            get => _selectedSocialNetwork;
            set
            {
                if (value != null)
                {
                    Link = value.Link;
                    IsObjectSelect = true;
                    _lastSelectedSocialNetworkId = value.Id;
                    ImageName = value.ImageName;
                }
                else IsObjectSelect = false;

                IsAddObject = false;
                IsObjectEdit = false;

                Set(ref _selectedSocialNetwork, value);
            }
        }
    }
}
