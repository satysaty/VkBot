using System.Threading.Tasks;

namespace VKBot.Core.Bot
{
    interface IBot
    {
        void Start();

        void Stop();  
        
        Task Work();        
    }
}
