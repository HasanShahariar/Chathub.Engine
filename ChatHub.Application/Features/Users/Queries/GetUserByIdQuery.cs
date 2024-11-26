using AutoMapper;
using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.Common.Models;
using ChatHub.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tailoring.Application.Com.Users.Queries.GetUsers;

namespace ChatHub.Application.Features.Users.Queries
{
    public class GetUserByIdQuery:IRequest<UserDto>
    {
        public int UserId { get; set; }
    }
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c=>c.Id==request.UserId, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
    }
}
