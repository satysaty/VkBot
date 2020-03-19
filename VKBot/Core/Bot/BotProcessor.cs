using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKBot.Abstractions;
using VKBot.Data;
using VKBot.Model;

namespace VKBot.Core.Bot
{
    class BotProcessor
    {
        DataContext DB;

        public Random Random { get; set; }

        public BotProcessor(DataContext db)
        {
            DB = db;

            Random = new Random();
        }

        public async Task Delay(int min, int max, CancellationToken token)
        {
            await Task.Delay(Random.Next(min * 60000, max * 60000), token);
        }

        public User GetNextUser()
        {
            User nextUser = DB.Users.Where(x => x.BlackList == false && (x.FriendStatus != FriendStatus.RequestSent && x.FriendStatus != FriendStatus.Friends)).FirstOrDefault();

            return nextUser;
        }

        public async Task DeleteUser()
        {
            DB.Users.Remove(GetNextUser());
            await DB.SaveChangesAsync();
        }

        public async Task<bool> CheckUser(ICollection<VkNet.Model.User> usersInfo, CheckUserParams userParams)
        {
            try
            {
                var info = usersInfo.FirstOrDefault();

                await CheckProfileExist(info.IsDeactivated);

                if (userParams.Status)
                    await CheckStatus(info.Status);
                if (userParams.LastSeen.Check)
                    await CheckLastSeen(info.LastSeen, userParams.LastSeen);
                //if (userParams.Friends.Check)
                //    await CheckFriends(info.FriendLists.Count(), userParams.Friends);
                if (userParams.Followers.Check)
                    await CheckFollowers(info.FollowersCount, userParams.Followers);
                if (userParams.NickName)
                    await CheckStatus(info.Nickname);
                if (!userParams.IsClosed)
                    await CheckClosed(info.IsClosed);
                if (!userParams.FollowOnly)
                    await CheckCanFriend(info.CanSendFriendRequest);

                await CheckInOutPutRequests(info.FriendStatus);


                return true;
            }
            catch (Exception)
            {
                return false;
            }  
        }

        public async Task<bool> CheckStatus(string status)
        {
            var blackWords = DB.BlackListWords.ToList();

            foreach (var word in blackWords)
            {
                if (status.ToLower().Contains(word.Text.ToLower()))
                {
                    GetNextUser().BlackList = true;
                    GetNextUser().BlackListReason = BlackListReason.Seller;
                    await DB.SaveChangesAsync();
                    throw new Exception();
                }
            }

            return true;
        }

        public async Task<bool> CheckClosed(bool? isClosed)
        {
            if (isClosed.HasValue)
                if (!isClosed.Value)
                    return true;

            GetNextUser().BlackList = true;
            GetNextUser().BlackListReason = BlackListReason.NotMatch;
            await DB.SaveChangesAsync();
            throw new Exception();
        }

        public async Task<bool> CheckCanFriend(bool? canFriendRequest)
        {
            if (canFriendRequest.HasValue)
                if (canFriendRequest.Value)
                    return true;

            GetNextUser().BlackList = true;
            GetNextUser().BlackListReason = BlackListReason.NotMatch;
            await DB.SaveChangesAsync();
            throw new Exception();
        }

        public async Task<bool> CheckLastSeen(VkNet.Model.LastSeen lastSeen, LastSeen lastSeenParams)
        {
            if (lastSeen.Time == null || lastSeen.Time < lastSeenParams.Min)
            {
                GetNextUser().BlackList = true;
                GetNextUser().BlackListReason = BlackListReason.NotMatch;
                await DB.SaveChangesAsync();
                throw new Exception();
            }
            return true;
        }

        public async Task<bool> CheckFriends(int countFriends, CheckUserParams userParams)
        {
            if (!userParams.Friends.Check)
                return false;

            if (userParams.Friends.Max != 0)
                if (countFriends > userParams.Friends.Max)
                {
                    GetNextUser().BlackList = true;
                    GetNextUser().BlackListReason = BlackListReason.NotMatch;
                    await DB.SaveChangesAsync();
                    return false;
                }
                    
            if (userParams.Friends.Min != 0)
                if (countFriends < userParams.Friends.Min)
                {
                    GetNextUser().BlackList = true;
                    GetNextUser().BlackListReason = BlackListReason.NotMatch;
                    await DB.SaveChangesAsync();
                    return false;
                }
                    
            return true;
        }

        public async Task<bool> CheckInOutPutRequests(VkNet.Enums.FriendStatus friendStatus)
        {
            if (friendStatus == VkNet.Enums.FriendStatus.InputRequest || friendStatus == VkNet.Enums.FriendStatus.OutputRequest)
            {
                GetNextUser().BlackList = true;
                GetNextUser().BlackListReason = BlackListReason.AlreadyFollowOrFriend;
                await DB.SaveChangesAsync();
                throw new Exception();
            }

            return true;
        }

        public async Task<bool> CheckProfileExist(bool isActive)
        {
            if (isActive)
            {
                DB.Users.Remove(GetNextUser());
                await DB.SaveChangesAsync();
                throw new Exception();
            }
            return true;
        }

        public async Task<bool> CheckFollowers(long? countFollowers, Followers followersParams)
        {
            if (!countFollowers.HasValue)
                return true;

            if (followersParams.Max != 0)
                if (countFollowers > followersParams.Max)
                {
                    GetNextUser().BlackList = true;
                    GetNextUser().BlackListReason = BlackListReason.NotMatch;
                    await DB.SaveChangesAsync();
                    throw new Exception();
                }

            if (followersParams.Min != 0)
                if (countFollowers < followersParams.Min)
                {
                    GetNextUser().BlackList = true;
                    GetNextUser().BlackListReason = BlackListReason.NotMatch;
                    await DB.SaveChangesAsync();
                    throw new Exception();
                }

            return true;
        }

    }
}
