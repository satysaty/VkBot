using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKBot.Model
{
    public class Group
    {
        public int Id { get; set; }

        public string Pk { get; set; }

        public string Name { get; set; }

        public Account Account { get; set; }

        public ICollection<User> Users { get; set; }

        public Group()
        {
            Users = new List<User>();
        }
    }
}
