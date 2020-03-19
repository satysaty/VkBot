using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VKBot.Data;
using VKBot.Model;
using VKBot.Interfaces;

namespace VKBot.Core
{
    public class SqlLog : ILog
    {
        private DataContext DB { get; set; }

        public int AccountId { get; set; } = 0;
        
        public delegate void LogHandler(string txt);

        public event LogHandler OnLogAdded;

        public SqlLog(DataContext db)
        {
            DB = db;
        }

        public async void Add(string txt)
        {
            await AddToDB($"[Log]: {txt}");
        }

        public async void Error(string txt)
        {
            await AddToDB($"[Error]: {txt}");
        }

        public async void Warning(string txt)
        {
            await AddToDB($"[Warning]: {txt}");
        }

        public async void Info(string txt)
        {
            await AddToDB($"[INFO]: {txt}");
        }

        protected async virtual Task AddToDB(string txt)
        {
            Log Log = new Log() { Text = txt, AccountId = AccountId, Date = DateTime.Now };
            OnLogAdded?.Invoke(txt);
            DB.Log.Add(Log);
            DB.SaveChanges();
        }
    }
}
