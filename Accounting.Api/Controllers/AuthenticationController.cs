using Accounting.Api.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel userModel)
        {
            var userExists = await _userManager.FindByEmailAsync(userModel.EmailAddress);
            if(userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message ="User with that email is registered."
                    });
            }

            IdentityUser user = new IdentityUser()
            {
                Email = userModel.EmailAddress,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userModel.EmailAddress
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "Registration failed"
                    });
            }

            return Ok(new Response 
            {
                Status = "Success",
                Message ="User created"
            });
        }
    }
}
