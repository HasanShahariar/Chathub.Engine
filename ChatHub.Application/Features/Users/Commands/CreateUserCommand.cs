using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.Features.Users.Dtos;
using ChatHub.Domain.Entity.setup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Application.Features.Users.Commands;

public record CreateUserCommand : IRequest<string>
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    //public UserDto model { get; set; }
}
internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IAppDbContext _context;
    

    public CreateUserCommandHandler(IAppDbContext context)
    {
        _context = context;
    
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
            };
            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return user.FullName;

        }

        catch (Exception ex) { throw new Exception(ex.Message); }
    }
}
