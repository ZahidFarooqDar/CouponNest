using CouponNestAPI.CouponNest.SM;
using CouponNestAPI.CouponNest.SM.Token;

namespace CouponNestAPI.Interface
{
    public interface IUserProcess
    {
        Task<UserSM> SignUpAsync(UserSM signUpModel);
        Task<UserSM> GetUserById(int id);
        Task<List<UserSM>> GetAllUsers();
        Task<UserSM> UpdateUser(int id, UserSM objSM);
        Task<DeleteResponseSM> DeleteUserById(int id);
        Task<TokenResponseSM> LoginAsync(SignInModelSM signInModel);
    }
}
