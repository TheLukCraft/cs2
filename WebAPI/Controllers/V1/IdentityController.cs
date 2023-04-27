using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;
using WebAPI.SwaggerExamples.Responses;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSenderService emailSenderService;

        public IdentityController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IEmailSenderService emailSenderService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.emailSenderService = emailSenderService;
        }

        /// <summary>
        /// Registers the user in the system.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        /// <response code="200">User created successfully!</response>
        /// <response code="409">User already exists!</response>
        [ProducesResponseType(typeof(RegisterResponseStatus200), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RegisterResponseStatus409), StatusCodes.Status409Conflict)]
        [HttpPost()]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel register)
        {
            var userExists = await userManager.FindByNameAsync(register.UserName);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new Response
                {
                    Succeeded = false,
                    Message = "User already exists!"
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.UserName,
            };
            var result = await userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response<bool>
                {
                    Succeeded = false,
                    Message = "User creation failed! Please check user details and try again",
                    Errors = result.Errors.Select(x => x.Description)
                });
            }
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            await userManager.AddToRoleAsync(user, UserRoles.User);

            await emailSenderService.Send(user.Email, "Registration confirmation", EmailTemplate.WelcomeMessage, user);

            return Ok(new Response
            {
                Succeeded = true,
                Message = "User created successfully!"
            });
        }

        /// <summary>
        /// Registers the admin in the system.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        [HttpPost()]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterModel register)
        {
            var userExists = await userManager.FindByNameAsync(register.UserName);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new Response<bool>
                {
                    Succeeded = false,
                    Message = "User already exists!"
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.UserName,
            };
            var result = await userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response<bool>
                {
                    Succeeded = false,
                    Message = "User creation failed! Please check user details and try again",
                    Errors = result.Errors.Select(x => x.Description)
                });
            }
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            await userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new Response
            {
                Succeeded = true,
                Message = "User created successfully!"
            });
        }

        /// <summary>
        /// Logins the user into system.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        [HttpPost()]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(LoginModel login)
        {
            var user = await userManager.FindByNameAsync(login.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, login.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var userRoles = await userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new AuthSuccessResponse()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}