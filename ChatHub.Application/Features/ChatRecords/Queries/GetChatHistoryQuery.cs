using AutoMapper;
using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.Common.Models;
using ChatHub.Application.Features.ChatRecords.Dtos;
using ChatHub.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tailoring.Application.Com.Users.Queries.GetUsers;

namespace ChatHub.Application.Features.ChatRecords.Queries
{
    public class GetChatHistoryQuery : IRequest<ListResult<ChatHistoryReturnDto>>
    {
        public int SenderId { get; init; }
        public int ReceiverId { get; init; }
    }

    public class GetChatHistoryQueryHandler : IRequestHandler<GetChatHistoryQuery, ListResult<ChatHistoryReturnDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetChatHistoryQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ListResult<ChatHistoryReturnDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ChatHistories.Where(c=>(c.SenderId==request.SenderId || c.SenderId==request.ReceiverId) && (c.ReceiverId==request.ReceiverId || c.ReceiverId == request.SenderId)).AsNoTracking();

         

            // Paginate the results
            var paginatedResult = await query.ToPaginatedListAsync(1, 1000, cancellationToken);

            // Map to ChatHistoryReturnDto
            var ChatHistoryReturnDtos = _mapper.Map<List<ChatHistoryReturnDto>>(paginatedResult.Data);

            return new ListResult<ChatHistoryReturnDto>
            {
                Data = ChatHistoryReturnDtos,
                Count = paginatedResult.TotalCount,
                Succeeded = true
            };
        }
    }


}
