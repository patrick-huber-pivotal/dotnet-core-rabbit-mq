using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMqPcf.Web.Models
{
    public class SendMessageModel
    {
        public string HostName { get; set; }
        public string QueueName { get; set; }
        public string MessageBody { get; set; }
    }
}
