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
    public class UserProcess : IUserProcess
    {
        #region Properties
        private readonly ApiDbContext  _apiDbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        #endregion Properties

        #region Constructor
        public UserProcess(ApiDbContext apiDbContext, 
             IConfiguration configuration,
             IMapper mapper)
        {
             _apiDbContext = apiDbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        #endregion Constructor

        #region SignUp User
        public async Task<UserSM> SignUpAsync(UserSM signUpModel)
        {
            var existingUserWithEmail = await _apiDbContext.Users.Where(x => x.Email == signUpModel.Email).FirstOrDefaultAsync();
            var existingUserWithUserName = await _apiDbContext.Users.Where(x => x.UserName == signUpModel.UserName).FirstOrDefaultAsync();

            if (existingUserWithEmail != null)
            {
                // A user with the same email already exists, return a custom IdentityResult
                throw new CouponNestException(ExceptionTypeDM.UserAlreadyExists, "User already present... Login Instead Or Use Different EmailId");
            }

            if (existingUserWithUserName != null)
            {
                // A user with the same email already exists, return a custom IdentityResult
                throw new CouponNestException(ExceptionTypeDM.UserAlreadyExists, "User already present... Login Instead Or Use Different User Name");
            }

            if (signUpModel.RoleType != RoleTypeSM.EndUser)
            {
                throw new CouponNestException(ExceptionTypeDM.InvalidRoleType, "Invalid Role Type... Try Again");
            }
            var userDM = _mapper.Map<UserDM>(signUpModel);

            // Add both user and userDM to the respective contexts
            //await _userManager.CreateAsync(user, signUpModel.Password);
            await _apiDbContext.Users.AddAsync(userDM);
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
            var user =  _apiDbContext.Users.Where(x => x.UserName == signInModel.UsernameOrEmail || x.Email == signInModel.UsernameOrEmail )
                .FirstOrDefault();
            if (user != null)
            {
                // Check if the user's RoleType matches the desired RoleType
                if (user.RoleType == (RoleTypeDM)signInModel.RoleType)
                {
                    //var result = await _signInManager.PasswordSignInAsync(signInModel.UsernameOrEmail, signInModel.Password, false, false);
                    var result = _apiDbContext.Users.Where(x => (x.UserName == signInModel.UsernameOrEmail || x.Email == signInModel.UsernameOrEmail)
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
            throw new CouponNestException(ExceptionTypeDM.InvalidUserException, "User Details Not Found...Kindly Sign Up first.");
        }
        #endregion Login User

        #region CRUD
        public async Task<UserSM> GetUserById(int id)
        {
            var user = await _apiDbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new CouponNestException(ExceptionTypeDM.NotFoundException, "User Not Found");
            }
            return _mapper.Map<UserSM>(user);
        }

        public async Task<List<UserSM>> GetAllUsers()
        {
            var users = await _apiDbContext.Users.ToListAsync();
            if(users.Count == 0)
            {
                throw new CouponNestException(ExceptionTypeDM.NotFoundException, "Users Not Found");
            }
            return _mapper.Map<List<UserSM>>(users);
        }

        public async Task<UserSM> UpdateUser(int id, UserSM objSM)
        {
            var user = await _apiDbContext.Users.FindAsync(id); // Ensure you await the FindAsync method
            if (user == null)
            {
                throw new CouponNestException(ExceptionTypeDM.NotFoundException, "User Not Found");
            }

            var dm = _mapper.Map<UserDM>(user);
            _apiDbContext.Users.Update(user);
            if (await _apiDbContext.SaveChangesAsync() > 0) 
            {
                var res = await GetUserById(id);
                return res;
            }

            throw new CouponNestException(ExceptionTypeDM.NoChangesException, "Error in Updating User Details");
        }


        public async Task<DeleteResponseSM> DeleteUserById(int id)
        {
            var user = await _apiDbContext.Users.FindAsync(id); // Ensure you await the FindAsync method
            if (user != null)
            {
                _apiDbContext.Users.Remove(user);
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
