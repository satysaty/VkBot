using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKBot.Model;

namespace VKBot.Interfaces
{
    public interface ILog
    {
        void Add(string txt);

        void Error(string txt);

        void Warning(string txt);

        void Info(string txt);
    }
}
