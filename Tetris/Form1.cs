using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        public static int xLength = 10;
        private static int yLength = 20;
        Button[,] gridBtn = new Button[xLength,yLength];
        Label scoreLbl = new Label();
        public int score = 0;

        public int BlockStartX = xLength / 2 - 1; // 4, start blocks here and build out right.
        public int BlockStartY = 0; // Top of grid
        public Form1()
        {
            InitializeComponent();

            // Variables
            

            // Initialise Grid
            for (int x = 0; x < gridBtn.GetLength(0); x++) 
            {
                for (int y = 0; y< gridBtn.GetLength(1); y++)
                {
                    gridBtn[x,y] = new Button();
                    gridBtn[x,y].FlatStyle = FlatStyle.Flat;
                    gridBtn[x, y].FlatAppearance.BorderColor = Color.White;
                    gridBtn[x,y].SetBounds(40+(30 * x), 60+(30 * y), 30, 30);
                    gridBtn[x,y].BackColor = Color.PowderBlue; // Every "Powder Blue" cell is empty.
                    // gridBtn[x,y].Text = Convert.ToString(x + 1) + "," + (y+1);

                    gridBtn[x,y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(gridBtn[x,y]);
                }
            }

            // Initialise Labels
            scoreLbl.Text = "Score: " + score;
            scoreLbl.SetBounds(500,60,60,60);
            Controls.Add(scoreLbl);

            // GAME START

            // Creates a new block at the top of the grid.

            // Square

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    gridBtn[BlockStartX+x, BlockStartY+y].BackColor = Color.Coral;
                }
            }

        }

        private void ClearRow(int x)
        {
            for (int y = 0; y < yLength; y++)
            {
                gridBtn[x,y].BackColor = Color.PowderBlue;
            }
        }

        private void MoveDown(int x, int numRows)
        {
            for (int y = 0; y < yLength; y++)
            {
                gridBtn[x + numRows, y] = gridBtn[x,y];
                gridBtn[x, y].BackColor = Color.PowderBlue;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int x = xLength - 1; x >= 0; x--)
            {
                if (IsRowFull(x))
                {
                    ClearRow(x);
                    cleared++;
                } else if (cleared > 0)
                {
                    MoveDown(x, cleared);
                }
            }

            return cleared;
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && x < xLength && y >= 0 && y < yLength;
        }

        public bool IsEmpty(int x, int y)
        {
            return IsInside(x, y) && gridBtn[x, y].BackColor.Equals(Color.PowderBlue);
        }

        public bool IsRowEmpty(int x)
        {
            for (int y = 0; y < yLength; y++)
            {
                if (!(gridBtn[x, y].BackColor.Equals(Color.PowderBlue)))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowFull(int x)
        {
            for (int y = 0; y < yLength; y++)
            {
                if (gridBtn[x, y].BackColor.Equals(Color.PowderBlue))
                {
                    return false;
                }
            }

            return true;
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
