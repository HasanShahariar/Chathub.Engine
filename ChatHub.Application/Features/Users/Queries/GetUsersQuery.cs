using AutoMapper;
using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.Common.Models;
using ChatHub.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tailoring.Application.Com.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<ListResult<UserDto>>
{
    public string? SearchKey { get; set; }
    public int PageNo { get; init; }
    public int PageSize { get; init; }
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ListResult<UserDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ListResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(request.SearchKey))
        {
            query = query.Where(u => u.FullName.Contains(request.SearchKey));
        }

        // Paginate the results
        var paginatedResult = await query.ToPaginatedListAsync(request.PageNo, request.PageSize, cancellationToken);

        // Map to UserDto
        var userDtos = _mapper.Map<List<UserDto>>(paginatedResult.Data);

        return new ListResult<UserDto>
        {
            Data = userDtos,
            Count = paginatedResult.TotalCount,
            Succeeded = true
        };
    }
}
