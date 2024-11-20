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
    internal class BlogsVM : EditorVM
    {
        public BlogsVM()
        {
            Blogs = new(GetBlogsWithImage());
            SelectImage = new LamdaCommand(OnSelectImage, CanSelectImage);
        }

        private List<Blog> GetBlogsWithImage() =>
            Task.Run(async () => await UserContext.SPClient.Blogs.GetListAsync()).Result;

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
                await UserContext.SPClient.Blogs.DeleteByIdAsync(SelectedBlog!.Id.ToString())).Wait();
            Blogs = new(GetBlogsWithImage());
            IsObjectSelect = false;
            _lastSelectedBlogId = Guid.Empty;
        }

        protected override bool CanReturn(object p) => base.CanReturn(p);
        protected override void OnReturn(object p)
        {
            base.OnReturn(p);
            if (!IsAddObject)
            {
                Blogs = new(GetBlogsWithImage());
                if (_lastSelectedBlogId != Guid.Empty)
                {
                    SelectedBlog = Blogs.First(p => p.Id == _lastSelectedBlogId);
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
            BlogTransfer newBlog = new(Title, Description);

            if (IsAddObject)
            {
                Task.Run(async () =>
                    await UserContext.SPClient.Blogs.AddAsync(newBlog, fstream)).Wait();

                Blogs = new(GetBlogsWithImage());
            }
            else
            {
                Task.Run(async () =>
                     await UserContext.SPClient.Blogs.EditAsync(_selectedBlog.Id.ToString(), newBlog, fstream)).Wait();
                Blogs = new(GetBlogsWithImage());
            }
            SelectedBlog = null;
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
        private string _description = "";
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);

        }

        private string _pictureName = "";
        public string PictureName
        {
            get => _pictureName;
            set => Set(ref _pictureName, value);

        }

        private Guid _lastSelectedBlogId;


        private Blog? _selectedBlog;
        public Blog? SelectedBlog
        {
            get => _selectedBlog;
            set
            {

                if (value != null)
                {
                    Title = value.Title;
                    Description = value.Description;
                    _lastSelectedBlogId = value.Id;
                    PictureName = value.ImageName;
                    IsObjectSelect = true;
                }
                else IsObjectSelect = false;

                IsAddObject = false;
                IsObjectEdit = false;
                Set(ref _selectedBlog, value);
            }
        }

        private ObservableCollection<Blog> _Blogs;
        public ObservableCollection<Blog> Blogs
        {
            get => _Blogs;
            set => Set(ref _Blogs, value);
        }
    }
}
