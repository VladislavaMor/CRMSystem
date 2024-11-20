using Newtonsoft.Json;
using CRM_Helper;

namespace CRM_API.Data
{
    internal static class ContactsFile
    {
        private const string REPOSITORY = "DataFiles";

        private const string FILE_FULL_NAME = "Contacts.json";

        private static string CreatePath() => Path.Combine(REPOSITORY, FILE_FULL_NAME);

        public static async Task<Contact?> GetContactsAsync()
        {
            Contact? contacts;
            string path = CreatePath();
            using (StreamReader r = new(path))
            {
                string json = await r.ReadToEndAsync();
                contacts = JsonConvert.DeserializeObject<Contact>(json);
            }
            return contacts;
        }

        public static async Task<SocialMedia?> GetSocialMedia(Guid id)
        {
            var contacts = await GetContactsAsync() ?? throw new NullReferenceException();
            var socNets = contacts.SocialMedias ?? throw new NullReferenceException();
            return socNets.SingleOrDefault(s => s.Id == id);
        }

        public async static Task<bool> IsExcistSocialMediaById(Guid id)
        {
            var contacts = await GetContactsAsync();
            if (contacts == null || contacts.SocialMedias == null) return false;
            return contacts.SocialMedias.Exists(s => s.Id == id);
        }

        public async static Task EditMainContacts(ContactsTransfer transfer)
        {
            var contacts = await GetContactsAsync();

            contacts = new()
            {
                Adress = transfer.Adress,
                PhoneNumber = transfer.PhoneNumber,
                Email = transfer.Email,
                LinkToMapContructor = transfer.LinkToMapContructor,
                SocialMedias = contacts.SocialMedias
            };
            contacts.Save();
        }

        public async static Task EditSocialMedia(SocialMedia sc)
        {
            var contacts = await GetContactsAsync();
            if (contacts != null)
            {
                var index = contacts.SocialMedias.IndexOf(contacts.SocialMedias.First(s => s.Id == sc.Id));
                contacts.SocialMedias[index] = sc;
                contacts.Save();
            }
        }

        public async static Task AddSocialMedia(SocialMedia socialMedia, byte[] image)
        {
            var contacts = await GetContactsAsync();
            contacts?.SocialMedias?.Add(socialMedia);
            await ImageDirectory.SaveImageAsync(socialMedia, image);
            contacts?.Save();

        }

        public async static Task<SocialMedia> DeleteSocialMediaAsync(Guid id)
        {
            var contacts = await GetContactsAsync();
            var SocialMedia = contacts?.SocialMedias?.First(s => s.Id == id);
            contacts.SocialMedias.Remove(SocialMedia);

            contacts.Save();

            return SocialMedia;
        }

        private static void Save(this Contact contacts)
        {
            string contactsJson = JsonConvert.SerializeObject(contacts);
            File.WriteAllText(CreatePath(), contactsJson);
        }
    }
}
