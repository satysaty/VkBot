using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKBot.Abstractions;
using VKBot.Data;
using VKBot.Model;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;

namespace VKBot.Core.Bot
{
    class DailyFriends : VkBot
    {
        public BotProcessor BotProcessor { get; set; }

        public CheckUserParams UserParams { get; set; }

        public DailyFriends(DataContext db, VkApi api, BotProcessor botProcessor) : base (db, api)
        {
            BotProcessor = botProcessor;
            
            UserParams = new CheckUserParams();
        }

        public override async Task Work()
        {
            
            while (Running)
            {
                try
                {
                    User nextUser = BotProcessor.GetNextUser();

                    if (nextUser == null)
                        return;
                    
                    var userInfo = await Api.Users.GetAsync(new List<long> { nextUser.Pk }, ProfileFields.All);

                    if (!userInfo.First().IsClosed.Value)
                    {
                        var friends = await Api.Friends.GetAsync(new VkNet.Model.RequestParams.FriendsGetParams { UserId = nextUser.Pk });

                        var checkFriends = await BotProcessor.CheckFriends(friends.Count, UserParams);

                        if (!checkFriends)
                        {
                            await BotProcessor.Delay(1, 1, TokenSource.Token);
                            continue;
                        }      
                    }
                        
                    var checkResult = await BotProcessor.CheckUser(userInfo, UserParams);

                    if (!checkResult)
                    {
                        await BotProcessor.Delay(1, 1, TokenSource.Token);
                        continue;
                    }

                    AddFriendStatus friendStatus = await Api.Friends.AddAsync(nextUser.Pk);

                    if (friendStatus == AddFriendStatus.Sended)
                    {
                        DB.Users.Where(p => p.Id == nextUser.Id).FirstOrDefault().FriendStatus = Model.FriendStatus.RequestSent;
                        await DB.SaveChangesAsync();
                    }   

                    await BotProcessor.Delay(14, 15, TokenSource.Token);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); ;
                    Running = false;
                }
            }
        }

        public void UpdateUserParams(bool followers, bool friends, bool lastSeen, bool isClosed, bool followOnly, bool status, bool nickName,
            int followersMax, int followersMin, int friendsMax, int friendsMin, DateTime lastSeenMax, DateTime lastSeenMin)
        {
            UserParams.Followers.Check = followers;
            UserParams.Followers.Max = followersMax;
            UserParams.Followers.Min = followersMin;
            UserParams.Friends.Check = friends;
            UserParams.Friends.Max = friendsMax;
            UserParams.Friends.Min = friendsMin;
            UserParams.IsClosed = isClosed;
            UserParams.LastSeen.Check = lastSeen;
            UserParams.LastSeen.Max = lastSeenMax;
            UserParams.LastSeen.Min = lastSeenMin;
            UserParams.Status = status;
            UserParams.NickName = nickName;
            UserParams.FollowOnly = followOnly;
        }

    }
}