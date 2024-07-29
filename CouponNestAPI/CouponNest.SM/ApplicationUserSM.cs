using CouponNestAPI.CouponNest.SM.EnumsSM;

namespace CouponNestAPI.CouponNest.SM
{
    public class ApplicationUserSM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public RoleTypeSM RoleType { get; set; }
    }
}
