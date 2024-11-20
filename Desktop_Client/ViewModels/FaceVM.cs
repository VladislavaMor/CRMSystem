using CRM_Helper;
using System.Threading.Tasks;

namespace Desktop_Client
{
    internal class FaceVM : EditorVM
    {
        public string BackGroundImage { get; } = @"Images\Studying.jpg";

        public FaceVM()
        {
            Face = Task.Run(async () => await UserContext.SPClient.Face.GetAsync()).Result;
        }

        protected override bool CanSave(object p)
        {
            if (string.IsNullOrEmpty(Slogan) || string.IsNullOrEmpty(CallToAction))
            {
                return false;
            }
            return true;

        }
        protected override void OnSave(object p)
        {
            Task.Run(async () => await UserContext.SPClient.Face
                .EditAsync(new Face() { CallToAction = CallToAction, Slogan = Slogan })).Wait();

            Face = Task.Run(async () =>
                await UserContext.SPClient.Face.GetAsync()).Result;
            IsObjectEdit = false;
        }

        protected override void OnReturn(object p)
        {
            base.OnReturn(p);
            Face = Task.Run(async () =>
                await UserContext.SPClient.Face.GetAsync()).Result;
        }



        private string _slogan = "";
        public string Slogan
        {
            get => _slogan;
            set => Set(ref _slogan, value);

        }

        private string _opportunity = "";
        public string CallToAction
        {
            get => _opportunity;
            set => Set(ref _opportunity, value);

        }


        private Face _face;
        public Face Face
        {
            get => _face;
            set
            {
                if (value != null)
                {
                    Slogan = value.Slogan;
                    CallToAction = value.CallToAction;
                    IsObjectSelect = true;
                }
                else
                {
                    IsObjectSelect = false;
                    IsObjectEdit = false;
                }


                Set(ref _face, value);
            }

        }

        protected override void OnDelete(object p)
        {
            throw new System.NotImplementedException();
        }
    }
}
