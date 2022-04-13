using System.ComponentModel.DataAnnotations;

namespace AspProject.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        [MaxLength(60)]
        public string UserNameOrEmail { get; set; }
        [Required] 
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
