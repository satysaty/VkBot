using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKBot.Data;
using VKBot.Interfaces;
using VKBot.Model;
using VkNet;
using VkNet.Enums.Filters;

namespace VKBot.Core.Action
{
    public class Parser
    {
        ILog Log;

        VkApi Api;

        DataContext DB;

        public Parser(VkApi api, ILog log)
        {
            Log = log;

            DB = new DataContext();

            Api = api;
        }

        public async Task ParseGroup(string groupId)
        {
            var res = await Api.Groups.GetByIdAsync(null, groupId, GroupsFields.All);

            int? membersCount = res.FirstOrDefault().MembersCount;

            List<VkNet.Model.User> Members = new List<VkNet.Model.User>();

            for (int i = 0; i < membersCount; i += 1000)
            {
                var s = await Api.Groups.GetMembersAsync(new VkNet.Model.RequestParams.GroupsGetMembersParams() { GroupId = groupId, Offset = i, Count = 1000 , Fields = UsersFields.FriendLists });

                Members.AddRange(s);         
            }

            var users = Members.Select(p => new VKBot.Model.User { Pk = p.Id, FirstName = p.FirstName, LastName = p.LastName, FriendStatus = Model.FriendStatus.Empty }).ToList();

            // await Task.Run(() => DB.Users.BulkInsert(users));

            await DB.Users.BulkInsertAsync(users);
        }
    }
}
