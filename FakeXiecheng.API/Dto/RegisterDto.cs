using System.ComponentModel.DataAnnotations;

namespace FakeXiecheng.API.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "两次密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}