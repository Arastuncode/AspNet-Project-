using System.ComponentModel.DataAnnotations;

namespace AspProject.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(60)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
