using ChatHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Application.Features.Users.Commands
{
    public record UpdateUserCommand : IRequest<string>
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public IFormFile? ProfileImage { get; set; }  // Using IFormFile for profile image
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly IAppDbContext _context;

        public UpdateUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find the user to update
                var user = await _context.Users.FindAsync(request.Id);

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                // Update user details from request
                user.FullName = request.FullName ?? user.FullName;
                user.Email = request.Email ?? user.Email;
                user.MobileNo = request.MobileNo ?? user.MobileNo;

                // If a new profile image is uploaded, save it
                if (request.ProfileImage != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{request.ProfileImage.FileName}";
                    // Save the file in a directory (e.g., wwwroot/images)
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.ProfileImage.CopyToAsync(stream);
                    }

                    // Update the profile image URL in the user object
                    user.ProfileImageUrl = $"images/{fileName}";
                }

                // Save changes to database
                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                return user.FullName;  // Return updated user full name or any relevant message
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user: " + ex.Message);
            }
        }
    }
}