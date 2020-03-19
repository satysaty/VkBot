using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKBot.Abstractions
{
    public class CheckUserParams
    {
        public bool IsClosed { get; set; } = false;
        public bool FollowOnly { get; set; } = false;
        public bool Status { get; set; } = false;
        public Friends Friends;
        public Followers Followers;
        public LastSeen LastSeen;
        public bool NickName { get; set; } = false;
    }

    public struct Friends
    {
        public bool Check;
        public int Min;
        public int Max;
    }

    public struct Followers
    {
        public bool Check;
        public int Min;
        public int Max;
    }

    public struct LastSeen
    {
        public bool Check;
        public DateTime Min;
        public DateTime Max;
    }
}
