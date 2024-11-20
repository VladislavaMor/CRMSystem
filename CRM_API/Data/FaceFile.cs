using Newtonsoft.Json;
using CRM_Helper;

namespace CRM_API.Data
{
    public static class FaceFile
    {
        private const string REPOSITORY = "DataFiles";

        private const string FILE_FULL_NAME = "Face.json";

        private static string CreatePath() => Path.Combine(REPOSITORY, FILE_FULL_NAME);

        public static async Task<Face> GetAsync()
        {

            Face? face;
            string path = CreatePath();
            using (StreamReader r = new(path))
            {
                string json = await r.ReadToEndAsync();
                face = JsonConvert.DeserializeObject<Face>(json);
            }
            return face;
        }

        public static void Save(Face face)
        {
            string contactsJson = JsonConvert.SerializeObject(face);
            File.WriteAllText(CreatePath(), contactsJson);
        }
    }
}
