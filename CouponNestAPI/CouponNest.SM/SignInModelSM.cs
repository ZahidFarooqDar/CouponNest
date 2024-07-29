using CouponNestAPI.CouponNest.SM.EnumsSM;

namespace CouponNestAPI.CouponNest.SM
{
    public class SignInModelSM
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

        public RoleTypeSM RoleType { get; set; }
    }
}
