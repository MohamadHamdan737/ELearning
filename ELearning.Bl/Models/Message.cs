using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string? SenderName { get; set; }
        public string? Messages { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
