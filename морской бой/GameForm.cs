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
        string abc = "abcdefghij";
       
        
        public GameForm(List<string> coords_my, List<string> coords_enemy)
        {
            InitializeComponent();
            table_user.RowCount = 10;
            table_enemy.RowCount = 10;
          

            for (int i=0;i<10;i++)
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
