using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VKBot.Core;
using VKBot.Data;
using VKBot.Interfaces;
using VKBot.Model;
using VkNet;

namespace VKBot
{
    public partial class AuthView : Form
    {
        DataContext DB;

        ILog Log;

        MainView MainView;

        Auth Auth;

        public AuthView(DataContext db, SqlLog log, MainView mainView)
        {
            InitializeComponent();

            DB = db;
         
            Auth = new Auth(mainView.Api);

            MainView = mainView;

            log.OnLogAdded += OnLogAdded;
            
            Log = log;
        }

        private void OnLogAdded(string txt)
        {
            listBox1.Items.Add(txt);
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        private async void AuthView_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            comboBox1.ValueMember = nameof(Account);
            comboBox2.ValueMember = nameof(Account);

            progressBar1.Value += 30;
            progressBar1.Value += 47;
            var accounts = await Task.Run(() => DB.Accounts.ToArray());
            comboBox1.Items.AddRange(accounts);

            var tokens = await Task.Run(() => DB.Accounts.Where(x => x.Token != null).ToArray());
            comboBox2.Items.AddRange(tokens);
            progressBar1.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            Account item = box.SelectedItem as Account;

            textBox1.Text = item.Login;
            textBox2.Text = item.Password;
            textBox3.Text = item.Token;

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Account item = (Account)comboBox2.SelectedItem;
            
            if (item == null)
            {
                Log.Info("You cannot select token");
                return;
            }

            try
            {
                Log.Info($"Authorize token: {item.Token}");
                var res = await Auth.Token(item.Token);

                if (res)
                {
                    var profileInfo = MainView.Api.Account.GetProfileInfo();
                    MainView.label3.Text = profileInfo.FirstName + " " + profileInfo.LastName;
                    if (profileInfo.Country != null)
                        MainView.label4.Text = profileInfo.Country.Title;
                    else
                        MainView.label4.Text = "No Country";
                    Log.Info($"Authorized as: {profileInfo.FirstName} {profileInfo.LastName}");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                Log.Info("Enter auth data");
                return;
            }

            try
            {
                Log.Info($"Authorize login: {textBox1.Text}");
                var res = await Auth.Login(textBox1.Text, textBox2.Text);

                if (res)
                {
                    var profileInfo = MainView.Api.Account.GetProfileInfo();
                    MainView.label3.Text = profileInfo.FirstName +" "+ profileInfo.LastName;
                    if (profileInfo.Country != null)
                        MainView.label4.Text = profileInfo.Country.Title;
                    else
                        MainView.label4.Text = "No Country";

                    Log.Info($"Authorized as: {profileInfo.FirstName} {profileInfo.LastName}");
                    this.Close();
                }
                    
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
