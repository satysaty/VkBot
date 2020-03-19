using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;

namespace VKBot.Model
{
    class Admin
    {
        public string Name { get; set; }

        public ulong Id { get; set; }

        public int Count { get; set; }

        public List<Message> Messages { get; set; }

        public Admin()
        {
            Messages = new List<Message>();

            Count = 0;
        }
        
        public void Up()
        {
            Count++;
        }
    }
}
