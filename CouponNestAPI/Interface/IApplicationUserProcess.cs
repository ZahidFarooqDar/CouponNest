using CouponNestAPI.CouponNest.SM;
using CouponNestAPI.CouponNest.SM.Token;

namespace CouponNestAPI.Interface
{
    public interface IApplicationUserProcess
    {
        Task<ApplicationUserSM> SignUpAsync(ApplicationUserSM signUpModel);
        Task<ApplicationUserSM> GetApplicationUserById(int id);
        Task<List<ApplicationUserSM>> GetAllApplicationUsers();
        Task<ApplicationUserSM> UpdateApplicationUser(int id, ApplicationUserSM objSM);
        Task<DeleteResponseSM> DeleteApplicationUserById(int id);
        Task<TokenResponseSM> LoginAsync(SignInModelSM signInModel);
    }
}
