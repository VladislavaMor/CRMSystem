using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CRM_API
{
    internal class AuthentificationOptions
    {
        public const string ISSUER = "SkillProfiServer"; // издатель токена
        public const string AUDIENCE = "SkillProfiClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 480; // время жизни токена 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
