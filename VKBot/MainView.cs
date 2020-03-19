using Autofac;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VKBot.Core;
using VKBot.Core.Action;
using VKBot.Core.Bot;
using VKBot.Core.Captcha;
using VKBot.Data;
using VKBot.Interfaces;
using VKBot.IoC;
using VKBot.Model;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using Group = VKBot.Model.Group;

namespace VKBot
{
    public partial class MainView : Form
    {
        public readonly VkApi Api;

        IContainer Container { get; set; }

        DataContext DB { get; set; }

        Parser Parser { get; set; }

        Account CurrentAccount { get; set; }

        DailyFriends DailyBot { get; set; }

        public MainView(IContainer container, VkApi api, DataContext db, Parser parser)
        {
            InitializeComponent();

            Container = container;

            Parser = parser;

            DB = db;

            DailyBot = Container.Resolve<DailyFriends>();

            Api = api;
        }

        private void OnLogAdded(string txt)
        {
            listBox1.Items.Add(txt);
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var Auth = Container.Resolve<AuthView>(new NamedParameter("mainView", this));

            Auth.ShowDialog();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var s = new CountMessage(Api);
            
            await s.Start(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = Container.Resolve<OnlineAlways>();

            s.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var s = Container.Resolve<OnlineAlways>();

            s.Stop();
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            var s = Container.Resolve<Parser>();

            await s.ParseGroup(textBox3.Text);
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(DB.Groups.ToArray());

            dateTimePicker3.Value = DateTime.Now.AddDays(-5);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            Group group = (Group)box.SelectedItem;

            textBox3.Text = group.Pk;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            DailyBot.UpdateUserParams(checkBox2.Checked, checkBox1.Checked, checkBox3.Checked,
                checkBox7.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked, 
                Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox7.Text), 
                Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox4.Text), 
                dateTimePicker4.Value, dateTimePicker3.Value);

            DailyBot.Start();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            DailyBot.Stop();
        }
    }
}
