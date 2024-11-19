using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Application.Features.ChatRecords.Dtos
{
    public class ChatHistoryReturnDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Messages { get; set; }
    }

    public class ChatHistoryCriteiaDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
