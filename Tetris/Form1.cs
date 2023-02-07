using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        public static int gridColumns = 10;
        public static int gridWidth = 10;
        public static int gridRows = 20;
        public static int gridHeight = 20;
        Button[,] gridBtn = new Button[gridColumns,gridRows];
        Label scoreLbl = new Label();
        public int score = 0;

        readonly Color[] colorList =
        {
            Color.CadetBlue, // I Block
            Color.Salmon, // L Block
            Color.DarkOliveGreen, // J Block
            Color.LightGreen, // S Block
            Color.Orange, // Z Block
            Color.Pink, // O Block
            Color.Orchid // T Block
        };

        public int BlockStartX = gridWidth / 2 - 1; // 4, start blocks here and build out right.
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
            DrawOBlock(BlockStartX, BlockStartY);

            Thread.Sleep(1000);
            MoveRowsDown(1, 2);

        }

        public void DrawOBlock(int drawX, int drawY)
        {
            int width = 2;
            int height = 2;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridBtn[drawX + x, drawY + y].BackColor = colorList[5];
                }
            }
        }

        private void ClearRow(int row)
        {
            for (int x = 0; x < gridColumns; x++)
            {
                gridBtn[x,row].BackColor = Color.PowderBlue;
            }
        }

        private void MoveRowsDown(int row, int numRows)
        {
            for (int x = 0; x < gridColumns; x++)
            {
                Console.WriteLine(row + "  " + x);
                Color setColor = gridBtn[row, x].BackColor;
                gridBtn[row + numRows, x].BackColor = setColor;
                gridBtn[row, x].BackColor = Color.PowderBlue;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int x = gridColumns - 1; x >= 0; x--)
            {
                if (IsRowFull(x))
                {
                    ClearRow(x);
                    cleared++;
                } else if (cleared > 0)
                {
                    MoveRowsDown(x, cleared);
                }
            }

            return cleared;
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
        }

        public bool IsEmpty(int x, int y)
        {
            return IsInside(x, y) && gridBtn[x, y].BackColor.Equals(Color.PowderBlue);
        }

        public bool IsRowEmpty(int x)
        {
            for (int y = 0; y < gridHeight; y++)
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
            for (int y = 0; y < gridHeight; y++)
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
