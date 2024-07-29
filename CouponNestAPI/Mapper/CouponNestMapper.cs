using AutoMapper;
using CouponNestAPI.CouponNest.DM;
using CouponNestAPI.CouponNest.DM.EnumsDM;
using CouponNestAPI.CouponNest.SM;
using CouponNestAPI.CouponNest.SM.EnumsSM;

namespace CouponNestAPI.Mapper
{
    public class CouponNestMapper : Profile
    {
        public CouponNestMapper() 
        {
            CreateMap<UserDM, UserSM>().ReverseMap();
            CreateMap<RoleTypeDM, RoleTypeSM>().ReverseMap();
        }
    }
}
