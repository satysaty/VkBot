using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKBot.Model;

namespace VKBot.Data.Initialize
{
    class AccountInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext db)
        {
            ICollection<Account> Accounts = new List<Account>
            {
                new Account()
                { 
                    Login="+3",
                    Password="BM3dXDeNE5aM",
                    Token="b3d6b5a6e6d0e6412c14"
                },
                new Account()
                {
                    Login="+3",
                    Password="0",
                    Token="4ae6c141"
                }
            };

            db.Accounts.AddRange(Accounts);

            ICollection<Group> Groups = new List<Group>
            {
                new Group()
                {
                    Account = Accounts.First(),
                    Name = "Trinity",
                    Pk = "67009979",
                }
            };

            db.Groups.AddRange(Groups);
        }
    }
}
