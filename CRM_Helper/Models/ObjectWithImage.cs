namespace CRM_Helper
{
    public class ObjectWithImage<T>
    {
        public ObjectWithImage() { }

        public T? Object { get; set; }

        public byte[]? Image { get; set; }
    }
}
