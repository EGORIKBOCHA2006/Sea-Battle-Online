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
        List<string> coords_my;
        List<string> coords_enemy;
        public GameForm(List<string> coords_my, List<string> coords_enemy, bool is_host, IPAddress ip_enemy = null)
        {
            InitializeComponent();
            table_user.RowCount = 10;
            table_enemy.RowCount = 10;
            iPAddressEnemy = ip_enemy;
            this.coords_my = coords_my;
            MessageBox.Show((is_host) ? "Хост" : "клиент");
            this.coords_enemy = coords_enemy;
            for (int i = 0; i < 10; i++)
            {
                table_user.Rows[i].Height = 40;
                table_enemy.Rows[i].Height = 40;
            }

            for (int i = 0; i < 10; i++)
            {
                //MessageBox.Show(abc[i].ToString());
                table_user.Rows[i].HeaderCell.Value = abc[i].ToString();
                table_enemy.Rows[i].HeaderCell.Value = abc[i].ToString();
            }

            foreach (string str in coords_my)
            {

                table_user[(int)char.GetNumericValue(str[1]), abc.IndexOf(str[0])].Style.BackColor = Color.BlueViolet;
            }
            main_game(is_host);

        }
        public async Task main_game(bool isHost)
        {
            int game_state;
            TcpListener server;
            TcpClient tcpClient;
            TcpClient client;
            NetworkStream stream;
           MessageBox.Show((isHost) ? "Хост" : "клиент"); 

            if (isHost)
            {
                game_state = 2;
                server = new TcpListener(IPAddress.Parse("192.168.31.86"), 8080);
                server.Start();
                tcpClient = await server.AcceptTcpClientAsync();
                stream = tcpClient.GetStream();
            }
            else
            {
                game_state = 3;
                client = new TcpClient();
                await client.ConnectAsync(iPAddressEnemy, 8080);
                stream = client.GetStream();
            }

            try
            {
                if (isHost)
                    await stream.WriteAsync(Encoding.UTF8.GetBytes("sefsef"), 0, (Encoding.UTF8.GetBytes("sefsef").Length));
                else
                {
                    byte[] data = new byte[1024];
                    await stream.ReadAsync(data, 0, data.Length);
                    string s = Encoding.UTF8.GetString(data);
                    MessageBox.Show(s);
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, (isHost) ? "Хост" : "клиент"); }
        }


    }
}
