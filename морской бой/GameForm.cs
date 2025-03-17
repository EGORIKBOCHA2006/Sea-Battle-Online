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
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace морской_бой
{
    public partial class GameForm : Form
    {
        string abc = "abcdefghij";
        IPAddress iPAddressEnemy;
        Shot shot;
        List<string> coords_my;
        List<string> coords_enemy;
        bool shout = false;
        bool is_host = false;

        public GameForm(List<string> coords_my, List<string> coords_enemy, bool is_host, IPAddress ip_enemy = null)
        {
            InitializeComponent();
            table_user.RowCount = 10;
            table_enemy.RowCount = 10;
            iPAddressEnemy = ip_enemy;
            this.coords_my = coords_my;
            this.is_host = is_host;
            this.coords_enemy = coords_enemy;

            for (int i = 0; i < 10; i++)
            {
                table_user.Rows[i].Height = 40;
                table_enemy.Rows[i].Height = 40;
            }

            for (int i = 0; i < 10; i++)
            {
                table_user.Rows[i].HeaderCell.Value = abc[i].ToString();
                table_enemy.Rows[i].HeaderCell.Value = abc[i].ToString();
            }

            foreach (string str in coords_my)
            {
                table_user[(int)char.GetNumericValue(str[1]), abc.IndexOf(str[0])].Style.BackColor = Color.BlueViolet;
            }

            Start_main_game();
        }

        public bool End_of_game()
        {
            return coords_my.Count == 0 || coords_enemy.Count == 0;
        }

        public async Task Start_main_game()
        {
            await Main_game(is_host);
        }

        public async Task end_shot()
        {
            while (!shout)
            {
                await Task.Delay(100); // Асинхронное ожидание
            }
            shout = false;
        }

        public async Task Wait_enemy(string type_enemy, NetworkStream stream)
        {
           
            while (true)
            {
                byte[] data = new byte[2048];
                int bytesRead = await stream.ReadAsync(data, 0, data.Length);
                if (bytesRead == 0)
                {
                    MessageBox.Show("Соединение с противником разорвано.");
                    break;
                }

                string json_string = Encoding.UTF8.GetString(data, 0, bytesRead).Trim('\0');
                MessageBox.Show(json_string);
                shot = JsonConvert.DeserializeObject<Shot>(json_string);

                if (shot.Sender == type_enemy)
                {
                    break;
                }
            }
        }

        public async Task Main_game(bool isHost)
        {
            int game_state;
            TcpListener server;
            TcpClient client;
            NetworkStream stream;

            if (isHost)
            {
                game_state = 2;
                server = new TcpListener(IPAddress.Parse("192.168.31.86"), 8080);
                server.Start();
                client = await server.AcceptTcpClientAsync();
                stream = client.GetStream();
            }
            else
            {
                game_state = 3;
                client = new TcpClient();
                await client.ConnectAsync(iPAddressEnemy, 8080);
                stream = client.GetStream();
            }

            do
            {
                if (game_state % 2 == 0)
                {
                    btnFire.Enabled = true;
                    await end_shot();
                    btnFire.Enabled = false;

                    string json_Shot = JsonConvert.SerializeObject(shot);
                    byte[] data = Encoding.UTF8.GetBytes(json_Shot);
                    await stream.WriteAsync(data, 0, data.Length);

                    if (shot.Hit)
                    {
                        game_state += 2;
                    }
                    else
                    {
                        game_state++;
                    }
                }
                else
                {
                    await Wait_enemy(isHost ? "client" : "server", stream);

                    if (shot.Hit)
                    {
                        MessageBox.Show("Перекрашен в красный");
                        table_user[shot.Nomber, abc.IndexOf(shot.Litera)].Style.BackColor = Color.Red;
                        coords_my.RemoveAt(coords_my.IndexOf(shot.Litera + shot.Nomber.ToString()));
                        game_state += 2;
                    }
                    else
                    {
                        MessageBox.Show("Перекрашен в Синий");
                        table_user[shot.Nomber, abc.IndexOf(shot.Litera)].Style.BackColor = Color.Blue;
                        game_state++;
                    }
                }
            } while (!End_of_game());

            MessageBox.Show("End");
        }

        private async void btnFire_Click(object sender, EventArgs e)
        {
            shot = new Shot(
                table_enemy.Rows[table_enemy.SelectedCells[0].RowIndex].HeaderCell.Value.ToString(),
                table_enemy.SelectedCells[0].ColumnIndex,
                (is_host)?"server":"client",
                coords_enemy.Contains(table_enemy.Rows[table_enemy.SelectedCells[0].RowIndex].HeaderCell.Value.ToString().ToLower() + table_enemy.SelectedCells[0].ColumnIndex.ToString())
            );
            shout = true;

            if (shot.Hit)
            {
                table_enemy.SelectedCells[0].Style.BackColor = Color.Red;
                coords_enemy.RemoveAt(coords_enemy.IndexOf(shot.Litera + shot.Nomber.ToString()));
            }
            else
            {
                table_enemy.SelectedCells[0].Style.BackColor = Color.Blue;
            }
        }

        public class Shot
        {
            public string Litera;
            public int Nomber;
            public string Sender;
            public bool Hit;

            public Shot(string litera, int nomber, string sender, bool hit)
            {
                this.Litera = litera;
                this.Nomber = nomber;
                this.Sender = sender;
                this.Hit = hit;
            }
        }
    }
}