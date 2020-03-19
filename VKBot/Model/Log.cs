using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKBot.Model
{
    public class Log
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}
