using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.Features.Auth.Models;
using ChatHub.Domain.Entity.setup;
using Google;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Application.Features.Auth.Commands
{
    public class RegisterCommand : IRequest<AuthResult>
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
    {
        private readonly IAppDbContext _context;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(IAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if the user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (existingUser != null)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "User already exists"
                };
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Save the new user
            var user = new User
            {
                FullName = request.FullName,
                UserName = request.Username,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResult
            {
                IsSuccess = true
            };
        }
    }

}
