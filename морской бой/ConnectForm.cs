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
        
        MakingMapForm makingMapF;
        public ConnectForm()
        {

            InitializeComponent();

        }

        /*private async void btn_Enter_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (rb_Client.Checked)
                {
                    TcpClient client = new TcpClient();
                    client.Connect(IPAddress.Parse(IPEnemy), 8080);
                    NetworkStream stream = client.GetStream();
                    StreamReader sr = new StreamReader(stream);
                    string message=await sr.ReadLineAsync();
                    MessageBox.Show(message);
                    client.Close();
                }
                else
                {
                    TcpListener server = new TcpListener(IPAddress.Parse("192.168.31.86"),8080);
                    server.Start();
                    await ListenerAsync();
                    MessageBox.Show("Server sent answer");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
        private async void btn_Enter_Click1(object sender, EventArgs e)
        {
            IPEnemy = IP.Text;
            try
            {
                if (rb_Client.Checked)
                {
                    TcpClient client = new TcpClient();
                    
                    try
                    {
                        await client.ConnectAsync(IPAddress.Parse(IPEnemy), 8080);
                   
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally

                    { 
                        client.Close(); 
                    }
                }
                else
                {
                    TcpListener server = new TcpListener(IPAddress.Parse("192.168.31.86"), 8080);
                    try
                    {
                        server.Start();
                        TcpClient client = await server.AcceptTcpClientAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        server.Stop();
                    }
        
                    
                }
           
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {             
                makingMapF = new MakingMapForm(this);
                this.Hide();
                makingMapF.ShowDialog();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
          
        }
        public async Task ListenerAsync()
        {


               //192.168.88.181
            return;
        }

        private void rb_Client_CheckedChanged(object sender, EventArgs e)
        {
            btn_Enter.Text = "Передать";
        }

        private void rb_Server_CheckedChanged(object sender, EventArgs e)
        {
            btn_Enter.Text = "Получить";
        }
        


    }
}