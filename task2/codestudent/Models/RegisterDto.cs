//using System.ComponentModel.DataAnnotations;

//namespace UserManagementAPI.Models
//{
//    public class RegisterDto
//    {
//        [Required]
//        public string Name { get; set; }

//        [Required]
//        public DateTime DateOfBirth { get; set; }

//        [Required]
//        [RegularExpression("^(Teacher|Student)$", ErrorMessage = "Designation must be Teacher or Student")]
//        public string Designation { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        [MinLength(6)]
//        public string Password { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Teacher|Student)$", ErrorMessage = "Designation must be Teacher or Student")]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
