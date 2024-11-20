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
    internal class ProjectsVM : EditorVM
    {
        public ProjectsVM()
        {
            Projects = new(GetProjects());
            SelectImage = new LamdaCommand(OnSelectImage, CanSelectImage);
        }

        private List<Project> GetProjects() =>
            Task.Run(async () => await UserContext.SPClient.Projects.GetListAsync()).Result;


        #region Commands

        #region Main Commands

        protected override bool CanAdd(object p) => base.CanAdd(p);
        protected override void OnAdd(object p)
        {
            base.OnAdd(p);

            Title = "Title";
            Description = "Description";
            PictureName = null;
        }


        protected override bool CanDelete(object p) => base.CanDelete(p);
        protected override void OnDelete(object p)
        {
            Task.Run(async () =>
                await UserContext.SPClient.Projects.DeleteByIdAsync(SelectedProject!.Id.ToString())).Wait();

            Projects = new(GetProjects());
            IsObjectSelect = false;
            _lastSelectedProjectId = Guid.Empty;
        }

        protected override bool CanReturn(object p) => base.CanReturn(p);
        protected override void OnReturn(object p)
        {
            base.OnReturn(p);
            if (!IsAddObject)
            {
                Projects = new(GetProjects());
                if (_lastSelectedProjectId != Guid.Empty)
                {
                    SelectedProject = Projects.First(p => p.Id == _lastSelectedProjectId);
                }

            }

        }

        protected override bool CanSave(object p)
        {
            if (IsAddObject && File.Exists(PictureName) && Title != string.Empty && Description != string.Empty)
                return true;

            else if (!IsAddObject && Title != string.Empty && Description != string.Empty)
                return true;

            return false;
        }

        protected override void OnSave(object p)
        {
            FileStream? fstream = File.Exists(PictureName) ? File.OpenRead(PictureName) : null;
            ProjectTransfer newProject = new()
            {
                Title = Title,
                Description = Description,
            };

            if (IsAddObject)
            {
                Task.Run(async () =>
                    await UserContext.SPClient.Projects.AddAsync(newProject, fstream)).Wait();

                Projects = new(GetProjects());
            }
            else
            {
                Task.Run(async () =>
                   await UserContext.SPClient.Projects.EditAsync(_lastSelectedProjectId.ToString(), newProject, fstream)).Wait();

                Projects = new(GetProjects());
            }

            SelectedProject = null;
            IsObjectEdit = false;
        }

        #endregion

        #region Original Commands
        private bool CanSelectImage(object p) => true;
        public ICommand SelectImage { get; }
        private void OnSelectImage(object p)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image Files (*.png, *.jpg)|*.png;*.jpg";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                PictureName = openFileDialog.FileName;

        }

        #endregion

        #endregion

        private string _title = "";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);

        }

        private string _pictureName = "";
        public string PictureName
        {
            get => _pictureName;
            set => Set(ref _pictureName, value);

        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);

        }

        private Guid _lastSelectedProjectId;


        private Project? _selectedProject;
        public Project? SelectedProject
        {
            get => _selectedProject;
            set
            {

                if (value != null)
                {
                    Title = value.Title;
                    Description = value.Description;
                    _lastSelectedProjectId = value.Id;
                    PictureName = value.ImageName;
                    IsObjectSelect = true;
                }
                else IsObjectSelect = false;

                IsAddObject = false;
                IsObjectEdit = false;
                Set(ref _selectedProject, value);
            }
        }

        private ObservableCollection<Project> _projects;
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set => Set(ref _projects, value);
        }

    }
}
