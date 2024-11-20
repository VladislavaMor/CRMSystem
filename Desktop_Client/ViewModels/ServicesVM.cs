using CRM_Helper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Desktop_Client
{
    internal class ServicesVM : EditorVM
    {
        public ServicesVM()
        {
            Services = new(Task.Run(async () =>
               await UserContext.SPClient.Services.GetListAsync()).Result);
        }

        protected override bool CanAdd(object p) => !IsObjectEdit;
        protected override void OnAdd(object p)
        {
            base.OnAdd(p);
            IsObjectSelect = true;
            Title = "Title";
            Description = "Description";
        }


        protected override bool CanDelete(object p)
        {
            return base.CanDelete(p);
        }
        protected override void OnDelete(object p)
        {
            Task.Run(async () =>
                await UserContext.SPClient.Services.DeleteByIdAsync(SelectedService!.Id.ToString())).Wait();

            Services = new(Task.Run(async () =>
               await UserContext.SPClient.Services.GetListAsync()).Result);

            IsObjectSelect = false;
            _lastSelectedProjectId = Guid.Empty;
        }


        protected override bool CanReturn(object p) => base.CanReturn(p);
        protected override void OnReturn(object p)
        {
            base.OnReturn(p);

            if (!IsAddObject)
            {
                Services = new(Task.Run(async () =>
                   await UserContext.SPClient.Services.GetListAsync()).Result);

                if (_lastSelectedProjectId != Guid.Empty)
                {
                    SelectedService = Services.First(p => p.Id == _lastSelectedProjectId);
                }

            }
        }


        protected override bool CanSave(object p)
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Description))
            {
                return false;
            }
            return true;
        }


        protected override void OnSave(object p)
        {
            ServiceTransfer newProject = new()
            {
                Title = Title,
                Description = Description,
            };

            if (IsAddObject)
            {
                Task.Run(async () =>
                   await UserContext.SPClient.Services.AddAsync(newProject)).Wait();

                Services = new(Task.Run(async () =>
                   await UserContext.SPClient.Services.GetListAsync()).Result);

            }
            else
            {
                SelectedService.Title = Title;
                SelectedService.Description = Description;

                Task.Run(async () =>
                    await UserContext.SPClient.Services.EditAsync(SelectedService.Id.ToString(), newProject)).Wait();

                Services = new(Task.Run(async () =>
                    await UserContext.SPClient.Services.GetListAsync()).Result);

            }

            SelectedService = null;
            IsObjectEdit = false;
        }

        private string _title = "";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);

        }
        private string _description = "";
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);

        }


        private Guid _lastSelectedProjectId;

        private Service _selectedServices;
        public Service SelectedService
        {
            get => _selectedServices;
            set
            {
                if (value != null)
                {
                    Title = value.Title;
                    Description = value.Description;
                    IsObjectSelect = true;
                    _lastSelectedProjectId = value.Id;
                }
                else IsObjectSelect = false;

                IsAddObject = false;
                IsObjectEdit = false;

                Set(ref _selectedServices, value);
            }

        }

        private ObservableCollection<Service> _services;
        public ObservableCollection<Service> Services
        {
            get => _services;
            set => Set(ref _services, value);
        }

    }
}
