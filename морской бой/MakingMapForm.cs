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
        bool statbro = false;
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
                server = new TcpListener(IPAddress.Parse("192.168.0.134"), 8080);
                server.Start();
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

        private void btnAccept_Click(object sender, EventArgs e)
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
                    client.Connect(new IPEndPoint(IPAddress.Parse(parentF.IP.Text), 8080));
                    NetworkStream stream = client.GetStream();
                    byte[] message = Encoding.Unicode.GetBytes("1");
                    stream.Write(message, 0, message.Length);
                    client.Close();

                    
                    server = new TcpListener(IPAddress.Parse("192.168.0.134"), 8080);
                    server.Start();
                    MessageBox.Show("Старт получений клиента");
                    Thread thread = new Thread(new ThreadStart(Listener));
                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();


                    this.Close();
                   

                }
                else
                {
                   while(!statbro)
                    {

                    }


                    MessageBox.Show("Отправка клиенту");
                    TcpClient client = new TcpClient();
                    client.Connect(new IPEndPoint(IPAddress.Parse(parentF.IP.Text), 8080));
                    MessageBox.Show("Сопряжение с клиентом");
                    NetworkStream stream = client.GetStream();
                    byte[] message = Encoding.Unicode.GetBytes("1");
                    stream.Write(message, 0, message.Length);
                    client.Close();

                    this.Close();
                    
                   
                }
                server.Stop();
                gameF = new GameForm(dataGridView1, (parentF.rb_Server.Checked) ? true : false, enemyString);
                gameF.MainGame();
                
                



             


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

                statbro = true;
                cl.Close();
                return;


            }
        }
    }
}
