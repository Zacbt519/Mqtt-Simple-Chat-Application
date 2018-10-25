using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        MqttClient myClient = new MqttClient("IPAddressHere", 1883, false, null, null, MqttSslProtocols.TLSv1_2);
        string message;
        string username;
        string password;
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            myClient.MqttMsgPublishReceived += client_receivedMessage;

            string clientId = Guid.NewGuid().ToString();
            myClient.Connect(clientId, "dev", "dev1234");

            myClient.Subscribe(new string[] {"Topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }

        private void client_receivedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            message = System.Text.Encoding.Default.GetString(e.Message);
            updateGui(message);
            Thread.Sleep(1000);
        }

        private void updateGui(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(updateGui), new object[] { value });
                return;
            }
            Message receivedMessage = JsonConvert.DeserializeObject<Message>(value);
            message += Environment.NewLine;
            richTextBox1.Text += receivedMessage.TimeSent + ": " +  receivedMessage.UserName + ": " + receivedMessage.UserMessage + Environment.NewLine;
            message = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string topic = "/wack/" + "test";
            Message usermsg = new Message()
            {
                UserMessage = txtMessage.Text,
                UserName = username
            };
            string json = JsonConvert.SerializeObject(usermsg);
            myClient.Publish(topic, Encoding.UTF8.GetBytes(json));
            txtMessage.Focus();
            txtMessage.Clear();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            username = txtUser.Text;
            password = txtPassword.Text;
            panel1.Visible = true;
            panel2.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}