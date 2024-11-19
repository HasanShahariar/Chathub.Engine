using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Domain.Entity.Chat
{
    [Table("CHATHUB_CHAT_HISTORY")]
    public class ChatHistory : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }  
        public bool IsRead { get; set; }  
    }
}
