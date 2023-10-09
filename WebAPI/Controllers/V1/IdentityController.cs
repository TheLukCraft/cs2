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
        /// Asynchronously registers a user.
        /// </summary>
        /// <param name="register">Registration data for the user.</param>
        /// <response code="409">(Conflict) if the user already exists.</response>
        /// <response code="400">(Bad Request) if the registration fails due to validation errors.</response>
        /// <response code="200">(OK) if the registration is successful.</response>
        [HttpPost()]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel register)
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
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            await userManager.AddToRoleAsync(user, UserRoles.User);

            await emailSenderService.SendAsync(user.Email, "Registration confirmation", EmailTemplate.WelcomeMessage, user);

            return Ok(new Response<bool>
            {
                Succeeded = true,
                Message = "User created successfully!"
            });
        }

        /// <summary>
        /// Asynchronously registers an admin user.
        /// </summary>
        /// <param name="register">Registration data for the admin user.</param>
        /// <returns>
        /// An IActionResult with an HTTP response containing information about the registration result:
        /// - Status 409 (Conflict) if the admin user already exists.
        /// - Status 400 (Bad Request) if the registration fails due to validation errors.
        /// - Status 200 (OK) if the admin user registration is successful.
        /// </returns>
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

            return Ok(new Response<bool>
            {
                Succeeded = true,
                Message = "User created successfully!"
            });
        }

        /// <summary>
        /// Asynchronously handles user login.
        /// </summary>
        /// <param name="login">Login credentials for the user.</param>
        /// <Response code="200">(OK) and a JWT token if the login is successful.</Response>
        /// <Response code="401">(Unauthorized) if the login fails.</Response>
        [HttpPost()]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(LoginModel login)
        {
            var user = await userManager.FindByNameAsync(login.UserName);

            if (user == null)
            {
                return NotFound(new Response<bool>
                {
                    Succeeded = false,
                    Message = "User not found."
                });
            }
            else if (user != null && await userManager.CheckPasswordAsync(user, login.Password))
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
            else return Unauthorized(new Response<bool>
            {
                Succeeded = false,
                Message = "Incorrect password."
            });
        }
    }
}