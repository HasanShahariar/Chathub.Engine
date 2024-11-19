﻿using ChatHub.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tailoring.Application.Com.Users.Queries.GetUsers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatHub.Web.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ApiControllerBase
{
    public UsersController(ISender sender) : base(sender)
    {

    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetUsersQuery query)
    {
        return Ok(await Mediator.Send(query));
    }


}