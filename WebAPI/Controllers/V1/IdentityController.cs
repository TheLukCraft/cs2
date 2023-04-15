using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IdentityController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost()]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var userExists = await userManager.FindByNameAsync(register.UserName);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
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
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
                {
                    Succeeded = false,
                    Message = "User creation failed! Please check user details and try again",
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            return Ok(new Response<bool>
            {
                Succeeded = true,
                Message = "User created successfully!"
            });
        }
    }
}