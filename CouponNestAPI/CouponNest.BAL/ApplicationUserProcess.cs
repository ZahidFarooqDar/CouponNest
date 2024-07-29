using AutoMapper;
using CouponNestAPI.CouponNest.DAL;
using CouponNestAPI.CouponNest.DM;
using CouponNestAPI.CouponNest.DM.EnumsDM;
using CouponNestAPI.CouponNest.SM;
using CouponNestAPI.CouponNest.SM.EnumsSM;
using CouponNestAPI.CouponNest.SM.Token;
using CouponNestAPI.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CouponNestAPI.CouponNest.BAL.Exception;
using CouponNestAPI.CouponNest.SM.Constants;

namespace CouponNestAPI.CouponNest.BAL
{
    public class ApplicationUserProcess : IApplicationUserProcess
    {
        #region Properties
        private readonly ApiDbContext  _apiDbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        #endregion Properties

        #region Constructor
        public ApplicationUserProcess(ApiDbContext apiDbContext, 
             IConfiguration configuration,
             IMapper mapper)
        {
             _apiDbContext = apiDbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        #endregion Constructor

        #region SignUp User
        public async Task<ApplicationUserSM> SignUpAsync(ApplicationUserSM signUpModel)
        {
            var existingApplicationUserWithEmail = await _apiDbContext.ApplicationUsers.Where(x => x.Email == signUpModel.Email).FirstOrDefaultAsync();
            var existingApplicationUserWithUserName = await _apiDbContext.ApplicationUsers.Where(x => x.UserName == signUpModel.UserName).FirstOrDefaultAsync();

            if (existingApplicationUserWithEmail != null)
            {
                // A user with the same email already exists, return a custom IdentityResult
                throw new CouponNestException(ExceptionTypeDM.UserAlreadyExists, "Application User already present... Login Instead Or Use Different EmailId");
            }

            if (existingApplicationUserWithUserName != null)
            {
                // A user with the same email already exists, return a custom IdentityResult
                throw new CouponNestException(ExceptionTypeDM.UserAlreadyExists, "Application User already present... Login Instead Or Use Different User Name");
            }

            /*if (signUpModel.RoleType != RoleTypeSM.SuperAdmin)
            {
                throw new CouponNestException(ExceptionTypeDM.InvalidRoleType, "Invalid Role Type... Try Again");
            }*/

            var userDM = _mapper.Map<ApplicationUserDM>(signUpModel);

            // Add both user and userDM to the respective contexts
            //await _userManager.CreateAsync(user, signUpModel.Password);
            await _apiDbContext.ApplicationUsers.AddAsync(userDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return signUpModel;
            }

            throw new CouponNestException(ExceptionTypeDM.GeneralException, "Something went wrong while your signUp... Please try Again Later");
        }

        #endregion SignUp User

        #region Login User
        public async Task<TokenResponseSM> LoginAsync(SignInModelSM signInModel)
        {
            // Check if the provided email and password are correct for the user
            var user =  _apiDbContext.ApplicationUsers.Where(x => x.UserName == signInModel.UsernameOrEmail || x.Email == signInModel.UsernameOrEmail )
                .FirstOrDefault();
            if (user != null)
            {
                // Check if the user's RoleType matches the desired RoleType
                if (user.RoleType == (RoleTypeDM)signInModel.RoleType)
                {
                    //var result = await _signInManager.PasswordSignInAsync(signInModel.UsernameOrEmail, signInModel.Password, false, false);
                    var result = _apiDbContext.ApplicationUsers.Where(x => (x.UserName == signInModel.UsernameOrEmail || x.Email == signInModel.UsernameOrEmail)
                    && x.Password == signInModel.Password).FirstOrDefault();

                    if (result != null)
                    {
                        // User is authenticated, generate and return the token

                        // Create claims for the JWT token
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, signInModel.UsernameOrEmail),
                            new Claim(ClaimTypes.Role, signInModel.RoleType.ToString()),
                            new Claim(ClaimTypes.Email,result.Email),
                            new Claim(ClaimTypes.GivenName,result.FirstName + " " + result.MiddleName + " " +result.LastName ),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimsRoot.Claim_DbRecordId, result.Id.ToString())
                        };

                        // Generate a key from your secret
                        var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

                        // Create a new JWT token
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddDays(30), // Token expiration time
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256Signature)
                        );

                        // Return the token as a string
                        var tokenResponse =  new JwtSecurityTokenHandler().WriteToken(token);
                        var res = new TokenResponseSM()
                        {
                            Token = tokenResponse
                        };
                        return res;
                    }
                }
                throw new CouponNestException(ExceptionTypeDM.TokenGenerationFailureException, "Something went wrong while generating Your Token... Please try Again Later");
            }
            throw new CouponNestException(ExceptionTypeDM.InvalidUserException, "Application User Details Not Found...Kindly Sign Up first.");
        }
        #endregion Login User

        #region CRUD
        public async Task<ApplicationUserSM> GetApplicationUserById(int id)
        {
            var user = await _apiDbContext.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                throw new CouponNestException(ExceptionTypeDM.NotFoundException, "User Not Found");
            }
            return _mapper.Map<ApplicationUserSM>(user);
        }

        public async Task<List<ApplicationUserSM>> GetAllApplicationUsers()
        {
            var users = await _apiDbContext.ApplicationUsers.ToListAsync();
            if(users.Count == 0)
            {
                throw new CouponNestException(ExceptionTypeDM.NotFoundException, "Users Not Found");
            }
            return _mapper.Map<List<ApplicationUserSM>>(users);
        }

        public async Task<ApplicationUserSM> UpdateApplicationUser(int id, ApplicationUserSM objSM)
        {
            var user = await _apiDbContext.ApplicationUsers.FindAsync(id); // Ensure you await the FindAsync method
            if (user == null)
            {
                throw new CouponNestException(ExceptionTypeDM.NotFoundException, "Application User Not Found");
            }

            var dm = _mapper.Map<ApplicationUserDM>(user);
            _apiDbContext.ApplicationUsers.Update(user);
            if (await _apiDbContext.SaveChangesAsync() > 0) 
            {
                var res = await GetApplicationUserById(id);
                return res;
            }

            throw new CouponNestException(ExceptionTypeDM.NoChangesException, "Error in Updating User Details");
        }


        public async Task<DeleteResponseSM> DeleteApplicationUserById(int id)
        {
            var user = await _apiDbContext.ApplicationUsers.FindAsync(id); // Ensure you await the FindAsync method
            if (user != null)
            {
                _apiDbContext.ApplicationUsers.Remove(user);
                if (await _apiDbContext.SaveChangesAsync() > 0) // Ensure you await the SaveChangesAsync method
                {
                    return new DeleteResponseSM() { IsDeleted = true, Message = "User Deleted Successfully" };
                }
                else
                {
                    throw new CouponNestException(ExceptionTypeDM.GeneralException, "Error in saving changes after deleting the user");
                }
            }

            throw new CouponNestException(ExceptionTypeDM.NotFoundException, "User not found");
        }

        #endregion CRUD

    }
}
