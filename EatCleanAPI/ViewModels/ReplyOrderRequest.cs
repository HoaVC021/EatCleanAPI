using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatCleanAPI.ViewModels
{
    public class ReplyOrderRequest
    {
        public int orderId { get; set; }
        public string ContentReply { get; set; }
    }
}
