using ChatHub.Application.Common.Interfaces;
using ChatHub.Application.Common.Models;
using ChatHub.Application.Features.Users.Commands;
using ChatHub.Domain.Entity.Chat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Application.Features.ChatRecords.Commands
{
    public class SaveChatHistoryCommand : IRequest<Result>
    {
        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public string Message { get; set; }

    }

    public class SaveChatHistoryCommandHandler : IRequestHandler<SaveChatHistoryCommand, Result>
    {
        private readonly IAppDbContext _context;



        public SaveChatHistoryCommandHandler(IAppDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(SaveChatHistoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = new Result();
                var chatHistory = new ChatHistory
                {
                    SenderId = request.SenderId,
                    ReceiverId = request.ReceiverId,
                    Message = request.Message,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };
                _context.ChatHistories.Add(chatHistory);

                await _context.SaveChangesAsync(cancellationToken);


                result.Succeeded = true;
                return result;

            }

            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }


}
