using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        int[] BlockCoordX;//Using these to store current Coordinates of the block
        int[] BlockCoordY;
        int currentColour;
        int spawn { get; set; }
        int nextSpawn { get; set; }

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
            BlockSpawn();

            Thread.Sleep(1000);
            MoveRowsDown(1, 2);

        }

        
        public void DrawIBlock(int drawX, int drawY)
        {
            int width = 4;
            for (int y = 0; y < width; y++)
            {
                gridBtn[drawX, drawY + y].BackColor = colorList[0];
                BlockCoordX[y] =  drawX;
                BlockCoordY[y] = drawY + y;
            }
            currentColour = 0;

        }
        public void DrawLBlock(int drawX, int drawY)
        {
            gridBtn[drawX,drawY].BackColor = colorList[1];
            gridBtn[drawX,drawY + 1].BackColor = colorList[1];
            gridBtn[drawX,drawY + 2].BackColor = colorList[1];
            gridBtn[drawX + 1, drawY + 2].BackColor = colorList[1];
            BlockCoordX[0] = drawX;
            BlockCoordY[0] = drawY;
            BlockCoordX[1] = drawX;
            BlockCoordY[1] = drawY + 1;
            BlockCoordX[2] = drawX;
            BlockCoordY[2] = drawY + 2;
            BlockCoordX[3] = drawX + 1;
            BlockCoordY[3] = drawY + 2;
            currentColour = 1;
        }
        public void DrawJBlock(int drawX, int drawY)
        {
            gridBtn[drawX, drawY].BackColor = colorList[2];
            gridBtn[drawX, drawY + 1].BackColor = colorList[2];
            gridBtn[drawX, drawY + 2].BackColor = colorList[2];
            gridBtn[drawX - 1, drawY + 2].BackColor = colorList[2];
            BlockCoordX[0] = drawX;
            BlockCoordY[0] = drawY;
            BlockCoordX[1] = drawX;
            BlockCoordY[1] = drawY + 1;
            BlockCoordX[2] = drawX;
            BlockCoordY[2] = drawY + 2;
            BlockCoordX[3] = drawX - 1;
            BlockCoordY[3] = drawY + 2;
            currentColour = 2;
        }
        public void DrawSBlock(int drawX, int drawY)
        {
            gridBtn[drawX, drawY].BackColor = colorList[3];
            gridBtn[drawX, drawY + 1].BackColor = colorList[3];
            gridBtn[drawX + 1, drawY + 1 ].BackColor = colorList[3];
            gridBtn[drawX + 1, drawY + 2].BackColor = colorList[3];
            BlockCoordX[0] = drawX;
            BlockCoordY[0] = drawY;
            BlockCoordX[1] = drawX;
            BlockCoordY[1] = drawY + 1;
            BlockCoordX[2] = drawX + 1;
            BlockCoordY[2] = drawY + 1;
            BlockCoordX[3] = drawX + 1;
            BlockCoordY[3] = drawY + 2;
            currentColour = 3;
        }
        public void DrawZBlock(int drawX, int drawY)
        {
            gridBtn[drawX, drawY].BackColor = colorList[4];
            gridBtn[drawX + 1, drawY].BackColor = colorList[4];
            gridBtn[drawX + 1, drawY + 1].BackColor = colorList[4];
            gridBtn[drawX + 2, drawY + 1].BackColor = colorList[4];
            BlockCoordX[0] = drawX;
            BlockCoordY[0] = drawY;
            BlockCoordX[1] = drawX + 1;
            BlockCoordY[1] = drawY;
            BlockCoordX[2] = drawX + 1;
            BlockCoordY[2] = drawY + 1;
            BlockCoordX[3] = drawX + 2;
            BlockCoordY[3] = drawY + 1;
            currentColour = 4;
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
                    BlockCoordX[x] = drawX + x;
                    BlockCoordY[y] = drawY + y;
                }
            }
            currentColour = 5;
        }
        public void DrawTBlock(int drawX, int drawY)
        {
            gridBtn[drawX, drawY + 1].BackColor = colorList[6];
            gridBtn[drawX + 1, drawY + 1].BackColor = colorList[6];
            gridBtn[drawX + 1, drawY].BackColor = colorList[6];
            gridBtn[drawX + 2, drawY + 1].BackColor = colorList[6];
            BlockCoordX[0] = drawX;
            BlockCoordY[0] = drawY + 1;
            BlockCoordX[1] = drawX + 1;
            BlockCoordY[1] = drawY + 1;
            BlockCoordX[2] = drawX + 1;
            BlockCoordY[2] = drawY;
            BlockCoordX[3] = drawX + 2;
            BlockCoordY[3] = drawY + 1;
            currentColour = 6;
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
                    score =+ 100;
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
        public bool BlockThere(int x, int y)//If there is a block where the coordinates are inputed it returns true  (Currently an issue with this is that the method might return true as sometimes the current block can set it off))
        {
            if (!gridBtn[x, y].BackColor.Equals(Color.PowderBlue))
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public void NextBlock(object sender, EventArgs e)
        {
            Random rdm = new Random();
            bool firstSpawn;//So that on first spawns the values are assigned
            do
            {
                spawn = rdm.Next(0, 6);//first spawn assigns the values
                nextSpawn = rdm.Next(0, 6);
                while (nextSpawn == spawn)//so that nextSpawn and spawn are not the same value
                {
                    nextSpawn = rdm.Next(0, 6);
                }
                firstSpawn = false;
            } while (firstSpawn == true);//once the original values are assigned it goes to normal
            
            if(firstSpawn == false)
            {
                spawn = nextSpawn;//the second time the method is called, the original spawn value is overwritten by next spawn
                nextSpawn = rdm.Next(0,6);//the nextSpawn value is assigned a random number
                while (nextSpawn == spawn)//so nextSpawn and spawn aren't the same
                {
                    nextSpawn = rdm.Next(0, 6);
                }
            }
            
        }
        public void MoveBlockLeft()
        {
            
            if (IsInside(BlockCoordX[0] -1, BlockCoordY[0]) == true || IsInside(BlockCoordX[1] - 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[2] - 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[3] - 1, BlockCoordY[0])  == true)
          
            { 
                for (int i = 0; i < 4; i++)
                {
                    BlockCoordX[i] = -1;//Decreasing each X value by one translates the block one to the left
                }
                DeleteCurrentBlock();//Deletes the block at the old coordinate
                for (int i = 0; i < 4; i++)
                {
                    gridBtn[BlockCoordX[i], BlockCoordY[i]].BackColor = colorList[currentColour];//spawns the block back in again at the new coordinate
                }
            }
        }
        public void MoveBlockRight() 
        {
            if (IsInside(BlockCoordX[0] + 1, BlockCoordY[0]) == false || IsInside(BlockCoordX[1] + 1, BlockCoordY[0]) == false || IsInside(BlockCoordX[2] + 1, BlockCoordY[0]) == false || IsInside(BlockCoordX[3] + 1, BlockCoordY[0]) == false)
            {
                for (int i = 0; i < 4; i++)
                {
                    BlockCoordX[i] = +1;//Increasing each X value by one translates the block one to the right
                }
                DeleteCurrentBlock();//Deletes the block at the old coordinate
                for (int i = 0; i < 4; i++)
                {
                    gridBtn[BlockCoordX[i], BlockCoordY[i]].BackColor = colorList[currentColour];//spawns the block back in again at the new coordinate
                }
            }
        }
        public void MoveBlockDown()
        {
            if(BlockThere(BlockCoordX[0], BlockCoordY[0] + 1) == false || BlockThere(BlockCoordX[0], BlockCoordY[0] + 1) == false || BlockThere(BlockCoordX[0], BlockCoordY[0] + 1) == false || BlockThere(BlockCoordX[0], BlockCoordY[0] + 1) == false)
            for (int i = 0; i < 4; i++)
            {
                BlockCoordY[i] = +1;//Increasing the Y coordinate means we can move the block down the grid
            }
            DeleteCurrentBlock();
            for (int i = 0; i < 4; i++)
            {
                gridBtn[BlockCoordX[i], BlockCoordY[i]].BackColor = colorList[currentColour];
            }

        }
        public void DeleteCurrentBlock()//Uses the block coordinates saved in an array to revert the block back to the base colour
        {
            for (int i = 0; i < 4; i++)
            {
                    gridBtn[BlockCoordX[i],BlockCoordY[i]].BackColor = Color.PowderBlue;
            }    
        }

        public void StoreBlock(object sender, EventArgs e)// enables us to store a block and switch to the next block 
        {
            int holder = spawn;
            spawn = nextSpawn;
            nextSpawn = holder;
            DeleteCurrentBlock();
            
        }
        public int DisplayScore()
        {
            return score;
        }
        public void BlockSpawn()
        {
            int blockSpawn = spawn;
            if (blockSpawn == 0)
            {
                DrawIBlock(BlockStartX, BlockStartY);
            }
            if (blockSpawn == 1)
            {
                DrawLBlock(BlockStartX, BlockStartY);
            }
            if (blockSpawn == 2)
            {
                DrawJBlock(BlockStartX, BlockStartY);   
            }
            if (blockSpawn == 3)
            {
                DrawSBlock(BlockStartX, BlockStartY);
            }
            if (blockSpawn == 4)
            {
                DrawZBlock(BlockStartX, BlockStartY);
            }
            if (blockSpawn == 5)
            {
                DrawOBlock(BlockStartX, BlockStartY);
            }
            if (blockSpawn == 6)
            {
                DrawTBlock(BlockStartX, BlockStartY);
            }
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
