using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace морской_бой
{
    
    public partial class MakingMapForm : Form
    {
        MakingMapForm makingmapF = null;
        int countFree = 20;
        int quadro = 1;
        int trio = 2;
        int duo = 3;
        int solo = 4;
        Thread thread;
        TcpListener server;
        bool server_ready;
        ConnectForm parentF;
        List<string> coords = new List<string>();
        List<string> coords_enemy = new List<string>();

        GameForm gameF;
        string enemyString;
        string abc = "ABCDEFGHIJ";
        public MakingMapForm(ConnectForm pF)
        {
            
            InitializeComponent();


            enemyString = pF.IP.Text;
            parentF = pF;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowCount = 10;
            for (int i = 0; i < 10; i++)
            {
                //MessageBox.Show(abc[i].ToString());
                dataGridView1.Rows[i].HeaderCell.Value = abc[i].ToString();
            }
            if (parentF.rb_Server.Checked)
            {
                this.Text = "Хост";
                server = new TcpListener(IPAddress.Parse("192.168.31.86"), 8080);
                server.Start();
               server_ready = false;
                ThreadStart threadstart = new ThreadStart(Listener);
                thread = new Thread(new ThreadStart(Listener));
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                this.Text = "Клиент";
            }
            

        }

   

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            
                switch(dataGridView1.SelectedCells.Count)
                {
                    case 4:
                    if (quadro > 0)               
                    {  
                        foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                        {
                            item.Style.BackColor = Color.BlueViolet;
                            item.Tag = "ship";
                        }
                        quadro--;
                        lblCount4.Text = quadro.ToString();
                    }
                        break;
                case 3:
                    if (trio > 0)
                    {
                        foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                        {
                            item.Style.BackColor = Color.BlueViolet;
                            item.Tag = "ship";
                        }
                        trio--;
                        lblCount3.Text = trio.ToString();
                    }
                    break;

                case 2:
                    if (duo > 0)
                    {
                        foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                        {
                            item.Style.BackColor = Color.BlueViolet;
                            item.Tag = "ship";
                        }
                        duo--;
                        lblCount2.Text = duo.ToString();
                    }
                    break;

                case 1:
                    if (solo > 0)
                    {
                        foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                        {
                            item.Style.BackColor = Color.BlueViolet;
                            item.Tag = "ship";
                        }
                        solo--;
                        lblCount1.Text = solo.ToString();
                    }
                    break;
                default:
                    MessageBox.Show("Неверное выстроен корабль");
                    break;
                }
            

 
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            if (lblCount1.Text == "0" && lblCount2.Text == "0" && lblCount3.Text == "0" && lblCount4.Text == "0")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[j].Tag != null)
                        {
                            coords.Add(abc[i].ToString().ToLower() + (j).ToString());
                        }
                    }
                }
                if (parentF.rb_Client.Checked)
                {
                    TcpClient client = new TcpClient();
                    await client.ConnectAsync(IPAddress.Parse(parentF.IP.Text), 8080);
                    NetworkStream stream = client.GetStream();
                    byte[] data = new byte[1024];
                     await stream.ReadAsync(data, 0, data.Length);
                    string data_string= Encoding.UTF8.GetString(data);
                    foreach(string item in data_string.Split(';'))
                        coords_enemy.Add(item);
                    string clientCoords = "";
                    foreach (string item in coords)
                    {
                        clientCoords += item + ";";
                    }
                    byte[] bytes = Encoding.UTF8.GetBytes("client;"+clientCoords);
                    stream.Write(bytes, 0, bytes.Length);
                    client.Close();

                    this.Hide();
                   
                    this.Close();



                } else
                {
                    server_ready = (quadro == 0 && trio == 0 && duo == 0 && solo == 0) ? true : false;
                }
            }
        }
        public void Listener()
        {
            TcpClient client;
            while (true)
            {
                client = server.AcceptTcpClient();
                if (client != null)
                    break;
            }
            while (true)
            {
                if (server_ready)
                {
                    NetworkStream stream = client.GetStream();
                    string serverCoords = "";
                    foreach (string item in coords)
                    {
                        serverCoords += item+";";
                    }
                    byte[] bytes = Encoding.UTF8.GetBytes(serverCoords);
                    stream.Write(bytes, 0, bytes.Length);
                    string first_element = "";
                    string client_coords="";
                    while (first_element!="client")
                    {
                        byte[] data = new byte[1024];                 
                        stream.Read(data, 0, data.Length);
                        client_coords = Encoding.UTF8.GetString(data);
                        first_element= client_coords.Split(';')[0];
                    }
                    for (int i = 1; i < client_coords.Split(';').Length; i++)
                        coords_enemy.Add(client_coords.Split(';')[i]);
                    break;
                }
            }
            this.Close();

        }
    }
}
