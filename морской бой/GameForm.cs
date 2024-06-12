using Newtonsoft.Json;
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
    
    public partial class GameForm : Form
    {
        List<string> coords;
        DataGridView myTable;
        int countMyShips = 20;
        int countEnemyShips = 20;
        bool isHost;
        TcpClient client;
        TcpListener server;
        bool fired = false;
        string enemyString;
        public GameForm(DataGridView parentDGV, bool isHost, string enemyString)
        {
            InitializeComponent();
            this.enemyString = enemyString;
            myTable = CopyDataGridView(parentDGV);
            this.isHost = isHost;
            this.myTable.AllowUserToAddRows = false;
            this.myTable.AllowUserToDeleteRows = false;
            this.myTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.myTable.Location = new System.Drawing.Point(12, 113);
            this.myTable.Name = "myTable";
            this.myTable.ReadOnly = true;
            this.myTable.Size = new System.Drawing.Size(581, 420);
            this.myTable.TabIndex = 1;
            this.Controls.Add(myTable);

            enemyTable.AllowUserToResizeRows = false;
            enemyTable.RowCount = 10;
            string abc = "ABCDEFGHIJ";
            for (int i = 0; i < 10; i++)
            {
                //MessageBox.Show(abc[i].ToString());
                enemyTable.Rows[i].HeaderCell.Value = abc[i].ToString();
            }

            myTable.Show();
            MessageBox.Show(isHost.ToString());
            this.Show();
        }

        private DataGridView CopyDataGridView(DataGridView dgv_org)
        {
            DataGridView dgv_copy = new DataGridView();
            try
            {
                if (dgv_copy.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in dgv_org.Columns)
                    {
                        dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }
                DataGridViewRow row = new DataGridViewRow();
                for (int i = 0; i < dgv_org.Rows.Count; i++)
                {
                    row = (DataGridViewRow)dgv_org.Rows[i].Clone();
                    int intColIndex = 0;
                    foreach (DataGridViewCell cell in dgv_org.Rows[i].Cells)
                    {
                        row.Cells[intColIndex].Value = cell.Value;
                        intColIndex++;
                    }
                    dgv_copy.Rows.Add(row);
                }
                dgv_copy.AllowUserToAddRows = false;
                dgv_copy.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Copy DataGridViw", ex.Message);
            }
            return dgv_copy;
        }


        public async void MainGame()
        {
            MessageBox.Show("мейн гейм");
            int stage = -1;
            if (isHost)
            {
                stage = 0;
            }
            else
            {
                stage = 1;
            }

            while (countEnemyShips>0 && countMyShips>0)
            {
                MessageBox.Show("Цикл");
                if (stage % 2 == 0)
                {
                    Application.DoEvents();
                    btnFire.Enabled = true;
                    while (!fired)
                    {
                        Application.DoEvents();
                    }
                    stage++;
                }
                else
                {
                    btnFire.Enabled = false;
                    server = new TcpListener(IPAddress.Parse("192.168.0.134"), 8080);
                    server.Start();
                    Thread thread = new Thread(new ThreadStart(Update));
                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();
                    server.Stop();
                    stage++;

                }
                fired = false;
            }
           
            









        }

        private void btn_Fire_Click(object sender, EventArgs e)
        {

            try
            {

                client = new TcpClient();
                client.Connect(IPAddress.Parse(enemyString), 8080);
                NetworkStream stream = client.GetStream();
                Shot shot = new Shot {x= enemyTable.SelectedCells[0].ColumnIndex + 1,
                y= enemyTable.Rows[enemyTable.SelectedCells[0].RowIndex].HeaderCell.Value.ToString(), flagHit=false, hit=false};
                string send=JsonConvert.SerializeObject(shot);
                //string send = "{\"x\":" + enemyTable.SelectedCells[0].ColumnIndex + 1 + ",\"y\":" + enemyTable.Rows[enemyTable.SelectedCells[0].RowIndex].HeaderCell.Value + ",\"hit\":" + false.ToString() + ",\"flagHit\":" + false.ToString() + "}";
                MessageBox.Show(send);
                byte[] message = Encoding.Unicode.GetBytes(send);
                stream.Write(message, 0, message.Length);
                client.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            fired = true;

        }
        new public async void Update()
        {
            while (true)
            {
                TcpClient cl = server.AcceptTcpClient();    //192.168.88.181
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
                return;
            }
        }


    }

    class Shot
    {
        public int x { get; set; }
        public string y { get; set; }
        public bool hit { get; set; }
        public bool flagHit { get; set; }
    }
}
