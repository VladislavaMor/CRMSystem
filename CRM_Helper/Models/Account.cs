using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class Account
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Максимальная длина логина не должна превышать 30 символов.")]
        [MinLength(5, ErrorMessage = "Минимальная длина логина должна быть 5 символов.")]
        public string Login { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Длина пароля должна быть менее 30 символов.")]
        [MinLength(5, ErrorMessage = "Минимальная длина пароля - 5 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
