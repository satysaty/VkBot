using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKBot.Interfaces;
using VkNet;

namespace VKBot.Core.Action
{
    class OnlineAlways
    {
        CancellationToken Token;

        CancellationTokenSource TokenSource;

        VkApi Api;

        MainView MainView;

        public OnlineAlways(VkApi api, MainView mainView)
        {
            Api = api;

            MainView = mainView;
        }

        public async void On(CancellationToken token)
        {
            while (true)
            {
                try
                { 
                    await Api.Account.SetOnlineAsync();
                    Console.WriteLine("Set Online");
                    await Task.Delay(270000, token);
                }
                catch (TaskCanceledException ex)
                {
                    MainView.label7.Text = "Off";
                    Console.WriteLine("Stop Online");
                    break;
                }
                catch (Exception ex)
                {
                    MainView.label7.Text = "Off";
                    Console.WriteLine(ex.Message);
                    break;
                }

            }
        }

        public async void Start()
        {
            Console.WriteLine("Start Online");

            MainView.label7.Text = "On";
            TokenSource = new CancellationTokenSource();

            Token = TokenSource.Token;

            On(Token);
        }

        public async void Stop()
        {
            TokenSource.Cancel();
        }
    }
}
