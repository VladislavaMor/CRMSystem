using System.Windows.Input;

namespace Desktop_Client
{
    internal abstract class EditorVM : ViewModel
    {
        public EditorVM()
        {
            Delete = new LamdaCommand(OnDelete, CanDelete);
            Edit = new LamdaCommand(OnEdit, CanEdit);
            Save = new LamdaCommand(OnSave, CanSave);
            Return = new LamdaCommand(OnReturn, CanReturn);
            Add = new LamdaCommand(OnAdd, CanAdd);
        }

        protected virtual bool CanAdd(object p)
        {
            if (IsAddObject || IsObjectEdit)
            {
                return false;
            }
            return true;
        }

        public ICommand Add { get; }
        protected virtual void OnAdd(object p)
        {
            IsObjectSelect = true;
            IsObjectEdit = true;
            IsAddObject = true;
        }


        protected virtual bool CanDelete(object p) => IsObjectSelect && !IsAddObject;
        public ICommand Delete { get; }
        protected abstract void OnDelete(object p);


        protected virtual bool CanEdit(object p) => !IsObjectEdit;
        public ICommand Edit { get; }
        protected virtual void OnEdit(object p)
        {
            IsObjectEdit = true;
        }


        protected virtual bool CanReturn(object p) => true;
        public ICommand Return { get; }
        protected virtual void OnReturn(object p)
        {
            IsAddObject = false;
            IsObjectEdit = false;
            IsObjectSelect = false;
        }


        protected virtual bool CanSave(object p) => true;
        public ICommand Save { get; }
        protected abstract void OnSave(object p);


        private bool _isObjectEdit = false;
        public bool IsObjectEdit
        {
            get => _isObjectEdit;
            set => Set(ref _isObjectEdit, value);

        }

        private bool _isAddObject = false;
        public bool IsAddObject
        {
            get => _isAddObject;
            set => Set(ref _isAddObject, value);
        }

        protected bool _isObjectSelect = false;
        public bool IsObjectSelect
        {
            get => _isObjectSelect;
            set => Set(ref _isObjectSelect, value);

        }

    }
}
