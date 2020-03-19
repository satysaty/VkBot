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

namespace VKBot.Core.Bot
{
    class VkBot : IBot
    {
        protected CancellationTokenSource TokenSource { get; set; }

        protected DataContext DB { get; set; }

        protected bool Running { get; set; }

        protected Account CurrentAccount { get; set; }

        protected VkApi Api { get; set; }

        public VkBot(DataContext db, VkApi api)
        {
            DB = db;

            Api = api;
        }

        public virtual async void Start()
        {
            CreateToken();

            Running = true;

            await Work();
        }

        public virtual Task Work()
        {
            throw new NotImplementedException();
        }

        public virtual void Stop()
        {
            Running = false;

            if (!TokenSource.IsCancellationRequested)
            {
                TokenSource.Cancel();
            }
        }

        protected void CreateToken()
        {
            TokenSource = new CancellationTokenSource();
        }
    }
}
