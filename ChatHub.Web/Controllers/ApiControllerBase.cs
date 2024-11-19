using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MediatR;
namespace ChatHub.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class ApiControllerBase : ControllerBase
    {

        private readonly ISender _mediator;

        protected ApiControllerBase(ISender mediator)
        {
            _mediator = mediator;
        }

        protected ISender Mediator => _mediator;
    }
}
