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
        bool serverReady;
        ConnectForm parentF;
        List<string> coords = new List<string>();
        GameForm gameF;
        string enemyString;
        public MakingMapForm(ConnectForm pF)
        {
            
            InitializeComponent();


            enemyString = pF.IP.Text;
            parentF = pF;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowCount = 10;
            string abc = "ABCDEFGHIJ";
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
               serverReady = false;
                ThreadStart threadstart = new ThreadStart(Listener);
                thread = new Thread(new ThreadStart(Listener));
                thread.IsBackground = true;
                thread.Start();
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

                foreach(DataGridViewCell cell in dataGridView1.SelectedCells)
                {

                    coords.Add(dataGridView1.Rows[cell.RowIndex].HeaderCell.Value.ToString()+(cell.ColumnIndex+1).ToString());
                }


                if (parentF.rb_Client.Checked)
                {
                    TcpClient client = new TcpClient();

                    await client.ConnectAsync(IPAddress.Parse(parentF.IP.Text), 8080);
                    NetworkStream stream = client.GetStream();
                    byte[] data = new byte[1024];
                    await stream.ReadAsync(data, 0, data.Length);
                    MessageBox.Show(Encoding.UTF8.GetString(data));
                    client.Close();



                    this.Close();



                } else
                {
                    serverReady = (quadro == 0 && trio == 0 && duo == 0 && solo == 0) ? true : false;
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
                if (serverReady)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] bytes = Encoding.UTF8.GetBytes("serverisready");
                    stream.Write(bytes, 0, bytes.Length);
                    break;
                }
            }
            this.Close();

        }
    }
}
