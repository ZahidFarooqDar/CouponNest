using CouponNestAPI.CouponNest.DM.EnumsDM;
using System.ComponentModel.DataAnnotations;

namespace CouponNestAPI.CouponNest.DM
{
    public class ApplicationUserDM
    {
        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required")]

        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role Type is required")]
        public RoleTypeDM RoleType { get; set; }
    }
}
