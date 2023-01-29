using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Button[,] gridBtn = new Button[10,20];
        Label scoreLbl = new Label();
        public int score = 0;
        public Form1()
        {
            InitializeComponent();
            
            // Initialise Grid
            for (int x = 0; x < gridBtn.GetLength(0); x++) 
            {
                for (int y = 0; y< gridBtn.GetLength(1); y++)
                {
                    gridBtn[x,y] = new Button();
                    gridBtn[x,y].FlatStyle = FlatStyle.Flat;
                    gridBtn[x, y].FlatAppearance.BorderColor = Color.White;
                    gridBtn[x,y].SetBounds(40+(30 * x), 60+(30 * y), 30, 30);
                    gridBtn[x,y].BackColor = Color.PowderBlue;
                    // gridBtn[x,y].Text = Convert.ToString(x + 1) + "," + (y+1);

                    gridBtn[x,y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(gridBtn[x,y]);
                }
            }

            // Initialise Labels
            scoreLbl.Text = "Score: " + score;
            scoreLbl.SetBounds(500,60,60,60);
            Controls.Add(scoreLbl);
        }

        void btnEvent_Click(object sender, EventArgs e)
        {
            Console.WriteLine(((Button)sender).Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
