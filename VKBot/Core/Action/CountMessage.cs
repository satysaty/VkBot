using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKBot.Interfaces;
using VKBot.Model;
using VkNet;
using VkNet.Model.RequestParams;

namespace VKBot.Core.Action
{
    class CountMessage
    {
        IEnumerable<User> Admins;

        VkApi Api;

        ILog Log;

        public CountMessage(VkApi api)
        {
            Api = api;
        }

        public async Task Start(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Count> Counts = new List<Count>();

                Admins = new List<User>()
                {
                new User(){ LastName ="", FirstName ="", Id=678},

                new User(){ LastName ="", FirstName ="", Id=678},
                };
                foreach (var admin in Admins)
                {
                    Counts.Add(new Count() { User = admin });
                }

                List<VkNet.Model.Conversation> conversations = new List<VkNet.Model.Conversation>();

                List<VkNet.Model.Message> messages = new List<VkNet.Model.Message>();

                for (int i = 0; i < 5; i++)
                {
                    var conversation = await Api.Messages.GetConversationsAsync(new GetConversationsParams() { GroupId = 678, Count = 200, Offset = Convert.ToUInt64(conversations.Count()) });

                    conversations.AddRange(conversation.Items.Select(x => x.Conversation));
                }

                foreach (var c in conversations)
                {
                    Console.WriteLine("-->" + c.Peer.Id);

                    var DialogHistory = await Api.Messages.GetHistoryAsync(new MessagesGetHistoryParams() { PeerId = c.Peer.Id, Count = 100, GroupId = 678 });

                    var msg = DialogHistory.Messages
                    .Where(x => x.Date > startDate)
                    .Where(x => x.Date < endDate.AddDays(1));

                    if (msg.Count() == 0)
                        break;
                    else
                    {
                        messages.AddRange(msg);
                    }
                }

                foreach (var item in messages)
                {
                    if (item.AdminAuthorId == null)
                        continue;



                    if (Counts.Where(x => x.User.Id == item.AdminAuthorId).FirstOrDefault() != null)
                        Counts.Where(x => x.User.Id == item.AdminAuthorId).FirstOrDefault().AddMessage();
                }

                foreach (var item in Counts.OrderByDescending(x => x.CountMessages))
                {
                    Console.WriteLine(item.User.FirstName + " " + item.User.LastName + ": " + item.CountMessages);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }
    } 
}

public class Count
{
    public User User { get; set; }

    public int CountMessages { get; set; } = 0;

    public void AddMessage()
    {
        CountMessages++;
    }
}

