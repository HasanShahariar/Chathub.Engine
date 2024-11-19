using ChatHub.Application.Features.ChatRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tailoring.Application.Com.Users.Queries.GetUsers;

namespace ChatHub.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : ApiControllerBase
    {
        public ChatController(ISender sender) : base(sender)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetChatHistoryQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
