using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.Features.Auth.Models;
using ChatHub.Domain.Entity.setup;
using Google.Apis.Admin.Directory.directory_v1.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Tailoring.Application.Common.Exceptions.ValidationException;

namespace ChatHub.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(ITokenService tokenService, IAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = new LoginResponse();

                var user = await _context.Users
                    .SingleOrDefaultAsync(q => q.UserName == request.Username, cancellationToken);

                if (user == null)
                {
                    throw new LoginFailedException();
                }
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                if (!isPasswordValid)
                {

                    throw new LoginFailedException();
                }
                result.AccessToken = new JwtSecurityTokenHandler().WriteToken(_tokenService.GenerateJWToken(user));
                result.RefreshToken = _tokenService.GenerateRefreshToken();
                result.UserId = user.Id;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
