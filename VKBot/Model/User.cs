using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;

namespace VKBot.Model
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long Pk { get; set; }

        public bool BlackList { get; set; }

        public BlackListReason BlackListReason { get; set; }

        public FriendStatus FriendStatus { get; set; }

        public Account Account { get; set; }

        public ICollection<Group> Groups { get; set; }

        public User()
        {
            Groups = new List<Group>();
        }
    }

    public enum BlackListReason
    {
        Empty,
        Closed,
        AlreadyFollowOrFriend,
        Seller,
        NotMatch
    }

    public enum FriendStatus
    {
        Empty,
        Friends,
        RequestSent
    }
}
