using CouponNestAPI.CouponNest.SM;
using CouponNestAPI.CouponNest.SM.Constants;
using CouponNestAPI.CouponNest.SM.Token;
using CouponNestAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponNestAPI.Controllers.ApplicationUsers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProcess _userProcess;

        public UserController(IUserProcess userProcess)
        {
            _userProcess = userProcess;
        }
        [HttpPost("signup")]
        public async Task<UserSM> SignUp([FromBody] UserSM signUpModel)
        {
            var result = await _userProcess.SignUpAsync(signUpModel);
            return result;
        }
        [HttpPost("login")]
        public async Task<TokenResponseSM> Login([FromBody] SignInModelSM signInModel)
        {
            var result = await _userProcess.LoginAsync(signInModel);

            return result;
        }

        [HttpGet("{id}")]
        [Authorize (Roles = "EndUser")]
        public async Task<UserSM> GetUserById(int id)
        {
            var dbRecordIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimsRoot.Claim_DbRecordId);

            var user = await _userProcess.GetUserById(id);
            return user;
        }

        [HttpGet]
        public async Task<IEnumerable<UserSM>> GetAllUsers()
        {
            var users = await _userProcess.GetAllUsers();
            return users;
        }

        [HttpPut("{id}")]
        public async Task<UserSM> UpdateUser(int id, [FromBody] UserSM objSM)
        {
            var updatedUser = await _userProcess.UpdateUser(id, objSM);
            return updatedUser;
        }

        [HttpDelete("{id}")]
        public async Task<DeleteResponseSM> DeleteUserById(int id)
        {
            var response = await _userProcess.DeleteUserById(id);
            return response;
        }
    }
}
