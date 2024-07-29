using CouponNestAPI.CouponNest.SM;
using CouponNestAPI.CouponNest.SM.Constants;
using CouponNestAPI.CouponNest.SM.Token;
using CouponNestAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponNestAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserProcess _applicationuserProcess;

        public ApplicationUserController(IApplicationUserProcess  applicationUserProcess)
        {
            _applicationuserProcess = applicationUserProcess;
        }
        [HttpPost("signup")]
        public async Task<ApplicationUserSM> SignUp([FromBody] ApplicationUserSM signUpModel)
        {
            var result = await _applicationuserProcess.SignUpAsync(signUpModel);
            return result;
        }
        [HttpPost("login")]
        public async Task<TokenResponseSM> Login([FromBody] SignInModelSM signInModel)
        {
            var result = await _applicationuserProcess.LoginAsync(signInModel);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApplicationUserSM> GetApplicationUserById(int id)
        {
            var dbRecordIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimsRoot.Claim_DbRecordId);

            var user = await _applicationuserProcess.GetApplicationUserById(id);
            return user;
        }

        [HttpGet]
        public async Task<IEnumerable<ApplicationUserSM>> GetAllApplicationUsers()
        {
            var users = await _applicationuserProcess.GetAllApplicationUsers();
            return users;
        }

        [HttpPut("{id}")]
        public async Task<ApplicationUserSM> UpdateUser(int id, [FromBody] ApplicationUserSM objSM)
        {
            var updatedUser = await _applicationuserProcess.UpdateApplicationUser(id, objSM);
            return updatedUser;
        }

        [HttpDelete("{id}")]
        public async Task<DeleteResponseSM> DeleteUserById(int id)
        {
            var response = await _applicationuserProcess.DeleteApplicationUserById(id);
            return response;
        }
    }
}
