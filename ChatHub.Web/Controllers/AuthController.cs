using ChatHub.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatHub.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        public AuthController(ISender sender) : base(sender)
        {

        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(LoginCommand model)
        {
            var result = await Mediator.Send(model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid request.");
            }

            // Call the RegisterCommand handler using MediatR
            var result = await Mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(new { Message = "User registered successfully!" });
        }
    }
}

