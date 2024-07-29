using CouponNestAPI.CouponNest.SM.EnumsSM;
using Microsoft.AspNetCore.Identity;

namespace CouponNestAPI.CouponNest.SM
{
    public class AuthenticUserSM : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public RoleTypeSM Role { get; set; }
    }
}
