using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace морской_бой
{
    public partial class ConnectForm : Form
    {
        string IPEnemy;
        TcpClient client;
        TcpListener server;
        MakingMapForm makingMapF;
        public ConnectForm()
        {
            
            InitializeComponent();
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (rb_Client.Checked)
                {
                    client = new TcpClient();
                    client.Connect(IPAddress.Parse(IPEnemy), 8080);
                    NetworkStream stream = client.GetStream();
                    string send = "{\"x\":" + textBox1.Text + ",\"y\":" + textBox2.Text + ",\"hit\":" + checkBox1.Checked + ",\"flagHit\":" + checkBox2.Checked + "}";
                    MessageBox.Show(send);
                    byte[] message = Encoding.Unicode.GetBytes("1");
                    stream.Write(message, 0, message.Length);
                    client.Close();
                }
                else
                {
                    server = new TcpListener(IPAddress.Parse(IPEnemy),8080);
                    server.Start();
                    Thread thread = new Thread(new ThreadStart(Update));
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        new public async void Update()
        {
            TcpClient cl = await server.AcceptTcpClientAsync();    //192.168.88.181
            StreamReader sr = new StreamReader(cl.GetStream(), Encoding.Unicode);
            string message = await sr.ReadLineAsync();
            MessageBox.Show(message);
            try
            {
                Shot shot = JsonConvert.DeserializeObject<Shot>(message);
                MessageBox.Show($"{shot.x} , {shot.y} , {shot.hit} , {shot.flagHit}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                server.Stop();
            }
        }

        private async void btn_Enter_Click1(object sender, EventArgs e)
        {
            IPEnemy = IP.Text;
            try
            {
                if (rb_Client.Checked)
                {
                   
                        client = new TcpClient();
                        client.Connect(new IPEndPoint(IPAddress.Parse(IPEnemy), 8080));
                        NetworkStream stream = client.GetStream();
                        byte[] message = Encoding.Unicode.GetBytes("1");
                        stream.Write(message, 0, message.Length);
                        client.Close();
                     
                


                    server = new TcpListener(IPAddress.Parse("192.168.0.134"), 8080);
                    server.Start();
                    Thread thread = new Thread(new ThreadStart(Listener));
                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();
                }
                else
                {
                    server = new TcpListener(IPAddress.Parse("192.168.0.134"), 8080);
                    server.Start();

                     Thread thread = new Thread(new ThreadStart(Listener));
                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();
                    
                    
                        client = new TcpClient();
                        await client.ConnectAsync(IPAddress.Parse(IPEnemy), 8080);
                 
                        NetworkStream stream = client.GetStream();
                        byte[] message = Encoding.Unicode.GetBytes("1");
                        stream.Write(message, 0, message.Length);
                        client.Close();
                    

                }

                server.Stop();


                makingMapF = new MakingMapForm(this);
                makingMapF.ShowDialog();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Listener()
        {
            while (true)
            {
                
                TcpClient cl = server.AcceptTcpClient();    //192.168.88.181

                StreamReader sr = new StreamReader(cl.GetStream(), Encoding.Unicode);
                string message = sr.ReadLine();
                //MSG.Text += "\n" + message;
               
  
                cl.Close();
                return;
                
               
            }
        }

        private void rb_Client_CheckedChanged(object sender, EventArgs e)
        {
            btn_Enter.Text = "Передать";
        }

        private void rb_Server_CheckedChanged(object sender, EventArgs e)
        {
            btn_Enter.Text = "Получить";
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            Update();
        }
    }
}