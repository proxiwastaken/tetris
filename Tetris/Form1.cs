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
using Timer = System.Windows.Forms.Timer;

namespace Tetris
{
    public partial class Form1 : Form
    {
        public static int gridColumns = 10;
        public static int gridWidth = 10;
        public static int gridRows = 20;
        public static int gridHeight = 20;
        private Timer timer = new Timer();
        Button[,] gridBtn = new Button[gridColumns, gridRows];
        Button rotateBtn = new Button();
        Button moveBtn = new Button();
        Button leftBtn = new Button();
        Button rightBtn = new Button();
        Label scoreLbl = new Label();
        Label NextBlockSpawn = new Label();
        public int score = 1;
        public static int[] BlockCoordX;//Using these to store current Coordinates of the block
        public static int[] BlockCoordY;
        int currentColour;
        int rotationState = 0;
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
                for (int y = 0; y < gridBtn.GetLength(1); y++)
                {
                    gridBtn[x, y] = new Button();
                    gridBtn[x, y].FlatStyle = FlatStyle.Flat;
                    gridBtn[x, y].FlatAppearance.BorderColor = Color.White;
                    gridBtn[x, y].SetBounds(40 + (30 * x), 60 + (30 * y), 30, 30);
                    gridBtn[x, y].BackColor = Color.PowderBlue; // Every "Powder Blue" cell is empty.
                    // gridBtn[x,y].Text = Convert.ToString(x + 1) + "," + (y+1);

                    gridBtn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(gridBtn[x, y]);
                }
            }

            // Initialise Controls
            rotateBtn.SetBounds(500, 90, 110, 70);
            rotateBtn.Text = "Rotate";
            rotateBtn.Click += new EventHandler(this.btnRotate_Click);
            Controls.Add(rotateBtn);

            moveBtn.SetBounds(500, 190, 110, 70);
            moveBtn.Text = "Move block down.";
            moveBtn.Click += new EventHandler(this.btnMove_Click);
            Controls.Add(moveBtn);

            leftBtn.SetBounds(500, 260, 50, 50);
            leftBtn.Text = "<";
            leftBtn.Click += new EventHandler(this.btnLeft_Click);
            Controls.Add(leftBtn);

            rightBtn.SetBounds(560,260,50,50);
            rightBtn.Text = ">";
            rightBtn.Click += new EventHandler(this.btnRight_Click);
            Controls.Add(rightBtn);

            // Initialise Labels
            scoreLbl.Text = "Score: " + score;
            scoreLbl.SetBounds(500, 60, 60, 60);
            Controls.Add(scoreLbl);
            NextBlockSpawn.Text = "Next Block: " + DisplayNextBlock(); ;
            NextBlockSpawn.SetBounds(500, 80, 60, 60);
            Controls.Add(NextBlockSpawn);

            // GAME START

            // Creates a new block at the top of the grid.
            bool gameLoop = true;
            BlockSpawn();
            timer.Interval = 2000;
            timer.Tick += new EventHandler(t_Tick);
            Thread.Sleep(1500);
            timer.Start();

        }

        public void t_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            MoveBlockDown();
            timer.Start();
        }

        public void DrawBlock()
        {
            for (int i = 0; i < 4; i++)
            {
                gridBtn[BlockCoordX[i], BlockCoordY[i]].BackColor = colorList[currentColour];//Draws Block at with the set coordinates
            }
        }
        public void DrawIBlock(int drawX, int drawY)
        {
            int width = 4;
            currentColour = 0;
            if (rotationState == 0)
            {
                for (int y = 0; y < width; y++)
                {
                    BlockCoordX[y] = drawX;
                    BlockCoordY[y] = drawY + y;
                }
                rotationState++;
            }
            else if (rotationState == 1)
            {
                for (int y = 0; y < width; y++)
                {
                    BlockCoordX[y] = (drawX - 1) + y;
                    BlockCoordY[y] = drawY;
                }
                rotationState--;
            }
        }


        public void DrawLBlock(int drawX, int drawY)
        {
            int width = 4;
            currentColour = 1;
            if (rotationState == 0)
            {
                BlockCoordX[0] = drawX;
                BlockCoordY[0] = drawY;
                BlockCoordX[1] = drawX;
                BlockCoordY[1] = drawY + 1;
                BlockCoordX[2] = drawX;
                BlockCoordY[2] = drawY + 2;
                BlockCoordX[3] = drawX + 1;
                BlockCoordY[3] = drawY + 2;
                rotationState++;
            }
            else if (rotationState == 1)
            {
                BlockCoordX[0] += 1;
                BlockCoordY[0] += 1;
                BlockCoordX[2] -= 1;
                BlockCoordY[2] -= 1;
                BlockCoordX[3] -= 2;
                rotationState++;
            }
            else if (rotationState == 2)
            {
                BlockCoordX[0] -= 1;
                BlockCoordY[1] -= 1;
                BlockCoordX[2] += 1;
                BlockCoordY[2] -= 2;
                BlockCoordX[3] += 2;
                BlockCoordY[3] -= 1;
                rotationState++;
            }
            else if (rotationState == 3)
            {
                BlockCoordX[0] -= 1;
                BlockCoordY[1] += 1;
                BlockCoordX[2] += 1;
                BlockCoordY[2] += 2;
                BlockCoordX[3] += 2;
                BlockCoordY[3] += 1;
                rotationState = 0;
            }
        }

        public void DrawJBlock(int drawX, int drawY)
        {
            currentColour = 2;
            int width = 4;
            if (rotationState == 0)
            {
                BlockCoordX[0] = drawX;
                BlockCoordY[0] = drawY;
                BlockCoordX[1] = drawX;
                BlockCoordY[1] = drawY + 1;
                BlockCoordX[2] = drawX;
                BlockCoordY[2] = drawY + 2;
                BlockCoordX[3] = drawX - 1;
                BlockCoordY[3] = drawY + 2;
                rotationState++;
            }
            else if (rotationState == 1)
            {
                BlockCoordX[0] += 1;
                BlockCoordY[0] += 2;
                BlockCoordY[1] += 1;
                BlockCoordX[2] -= 1;
                BlockCoordY[3] -= 1;
                rotationState++;
            }
            else if (rotationState == 2)
            {
                BlockCoordX[0] -= 1;
                BlockCoordY[1] -= 1;
                BlockCoordX[2] += 1;
                BlockCoordY[2] -= 2;
                BlockCoordX[3] += 2;
                BlockCoordY[3] -= 1;
                rotationState++;
            }
            else if (rotationState == 3)
            {
                BlockCoordX[0] -= 1;
                BlockCoordY[0] -= 1;
                BlockCoordX[2] += 1;
                BlockCoordY[2] += 1;
                BlockCoordY[3] += 2;
                rotationState = 0;
            }
            /*else if (rotationState == 4)
            {
                BlockCoordX[0] = -2;
                BlockCoordX[1] = -1;
                BlockCoordY[1] = +1;
                BlockCoordY[2] = +2;
                BlockCoordX[3] = +1;
                BlockCoordY[3] = +1;
                rotationState = 0; // resets the rotation state to 0, so when rotate is pressed it iterate and rotation state will go to one therefor drawing the original rotation
            }*/
        }
        public void DrawSBlock(int drawX, int drawY)
        {
            currentColour = 3;
            int width = 4;
            if (rotationState == 0)
            {
                BlockCoordX[0] = drawX;
                BlockCoordY[0] = drawY;
                BlockCoordX[1] = drawX;
                BlockCoordY[1] = drawY + 1;
                BlockCoordX[2] = drawX + 1;
                BlockCoordY[2] = drawY + 1;
                BlockCoordX[3] = drawX + 1;
                BlockCoordY[3] = drawY + 2;
                rotationState++;
            }
            else if (rotationState == 1)
            {
                BlockCoordX[0] += 2;
                BlockCoordY[0] += 1;
                BlockCoordX[1] += 1;
                BlockCoordY[2] += 1;
                BlockCoordX[3] -= 1;
                rotationState++;
            }
            else if (rotationState == 2)
            {
                BlockCoordX[0] -= 2;
                BlockCoordY[0] -= 1;
                BlockCoordX[1] -= 1;
                BlockCoordY[2] -= 1;
                BlockCoordX[3] += 1;
                rotationState = 1;
            }
            /*else if (rotationState == 3)
            {
                BlockCoordX[0] += 2;
                BlockCoordY[0] += 1;
                BlockCoordX[1] += 1;
                BlockCoordY[2] += 1;
                BlockCoordX[3] -= 1;
                rotationState = 2;
            }*/
        }
        public void DrawZBlock(int drawX, int drawY)
        {
            currentColour = 4;
            int width = 4;
            if (rotationState == 0)
            {
                BlockCoordX[0] = drawX;
                BlockCoordY[0] = drawY;
                BlockCoordX[1] = drawX;
                BlockCoordY[1] = drawY + 1;
                BlockCoordX[2] = drawX - 1;
                BlockCoordY[2] = drawY + 1;
                BlockCoordX[3] = drawX - 1;
                BlockCoordY[3] = drawY + 2;
                rotationState++;
            }
            else if (rotationState == 1)
            {
                BlockCoordY[0] += 2;
                BlockCoordX[1] -= 1;
                BlockCoordY[1] += 1;
                BlockCoordX[3] -= 1;
                BlockCoordY[3] -= 1;
                rotationState++;
            }
            else if (rotationState == 2)
            {
                BlockCoordY[0] -= 2;
                BlockCoordX[1] += 1;
                BlockCoordY[1] -= 1;
                BlockCoordX[3] += 1;
                BlockCoordY[3] += 1;
                rotationState = 1; // resets the rotation state to 0, so when rotate is pressed it iterate and rotation state will go to one therefor drawing the original rotation
            }
        }
        public void DrawOBlock(int drawX, int drawY)
        {
            currentColour = 5;
            int width = 4;
            BlockCoordX[0] = drawX;
            BlockCoordY[0] = drawY;
            BlockCoordX[1] = drawX + 1;
            BlockCoordY[1] = drawY;
            BlockCoordX[2] = drawX;
            BlockCoordY[2] = drawY + 1;
            BlockCoordX[3] = drawX + 1;
            BlockCoordY[3] = drawY + 1;


        }
        public void DrawTBlock(int drawX, int drawY)
        {
            currentColour = 6;
            int width = 4;
            if (rotationState == 0)
            {
                BlockCoordX[0] = drawX;
                BlockCoordY[0] = drawY;
                BlockCoordX[1] = drawX;
                BlockCoordY[1] = drawY + 1;
                BlockCoordX[2] = drawX + 1;
                BlockCoordY[2] = drawY + 1;
                BlockCoordX[3] = drawX;
                BlockCoordY[3] = drawY + 2;
                rotationState++;
            }
            else if (rotationState == 1)
            {
                // started to compress rotation code starting here, can get confusing with the coordinates. atleast it works.
                BlockCoordX[0] -= 1;
                BlockCoordY[0] += 1;
                rotationState++;
            }
            else if (rotationState == 2)
            {
                BlockCoordX[2] -= 1;
                BlockCoordY[2] -= 1;
                rotationState++;
            }
            else if (rotationState == 3)
            {
                BlockCoordX[3] += 1;
                BlockCoordY[3] -= 1;
                rotationState++;
            }
            else if (rotationState == 4)
            {
                BlockCoordX[0] += 1;
                BlockCoordY[0] -= 1;
                BlockCoordX[2] += 1;
                BlockCoordY[2] += 1;
                BlockCoordX[3] -= 1;
                BlockCoordY[3] += 1;
                rotationState = 1; 
            }
            else if (rotationState == 5)
            {
                BlockCoordX[0] -= 1;
                BlockCoordY[0] += 1;
                rotationState = 1;
            }
        }


        private void ClearRow(int row)
        {
            for (int x = 0; x < gridColumns; x++)
            {
                gridBtn[x, row].BackColor = Color.PowderBlue;
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
                    score = +100;
                }
                else if (cleared > 0)
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
                for (int z = 0; z < 4; z++)
                {
                    if (x == BlockCoordX[z] && y == BlockCoordY[z])
                    {
                        return false;
                    }
                }
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
        public void NextBlock()
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

            if (firstSpawn == false)
            {
                spawn = nextSpawn;//the second time the method is called, the original spawn value is overwritten by next spawn
                nextSpawn = rdm.Next(0, 6);//the nextSpawn value is assigned a random number
                while (nextSpawn == spawn)//so nextSpawn and spawn aren't the same
                {
                    nextSpawn = rdm.Next(0, 6);
                }
            }

        }
        public void MoveBlockLeft()
        {

            if (IsInside(BlockCoordX[0] - 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[1] - 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[2] - 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[3] - 1, BlockCoordY[0]) == true)
            {
                DeleteCurrentBlock();//Deletes the block at the old coordinate
                for (int i = 0; i < 4; i++)
                {
                    BlockCoordX[i] -= 1;//Decreasing each X value by one translates the block one to the left
                }
                DrawBlock();
            }
        }
        public void MoveBlockRight()
        {
            if (IsInside(BlockCoordX[0] + 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[1] + 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[2] + 1, BlockCoordY[0]) == true || IsInside(BlockCoordX[3] + 1, BlockCoordY[0]) == true)
            {

                DeleteCurrentBlock();//Deletes the block at the old coordinate
                for (int i = 0; i < 4; i++)
                {
                    BlockCoordX[i] += 1; //Increasing each X value by one translates the block one to the right
                }
                DrawBlock();
            }
        }
        public void MoveBlockDown()
        {
            if (BlockCoordY[0] + 1 == 20 || BlockCoordY[1] + 1 == 20 || BlockCoordY[2] + 1 == 20 ||
                BlockCoordY[3] + 1 == 20)
            {
                SettleBlock();
                BlockSpawn();
                return;
            }
            if (BlockThere(BlockCoordX[0], BlockCoordY[0] + 1) == false ||
                BlockThere(BlockCoordX[1], BlockCoordY[1] + 1) == false ||
                BlockThere(BlockCoordX[2], BlockCoordY[2] + 1) == false ||
                BlockThere(BlockCoordX[3], BlockCoordY[3] + 1) == false)
            {
                if (BlockThere(BlockCoordX[0], BlockCoordY[0] + 1) == true ||
                    BlockThere(BlockCoordX[1], BlockCoordY[1] + 1) == true ||
                    BlockThere(BlockCoordX[2], BlockCoordY[2] + 1) == true ||
                    BlockThere(BlockCoordX[3], BlockCoordY[3] + 1) == true)
                {
                    SettleBlock();
                    BlockSpawn();
                    return;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        DeleteCurrentBlock();
                        BlockCoordY[i] += 1;//Increasing the Y coordinate means we can move the block down the grid
                        DrawBlock();
                    }
                }
            }
        }

        public void SettleBlock()
        {
            for (int x = 0; x < 4; x++)
            {
                BlockCoordX[x] = 0;
                BlockCoordY[x] = 0;
            }
        }

        public void DeleteCurrentBlock()//Uses the block coordinates saved in an array to revert the block back to the base colour
        {
            for (int i = 0; i < 4; i++)
            {
                gridBtn[BlockCoordX[i], BlockCoordY[i]].BackColor = Color.PowderBlue;
                Console.WriteLine(gridBtn[BlockCoordX[i], BlockCoordY[i]].BackColor);
            }
        }

        public void StoreBlock()// enables us to store a block and switch to the next block 
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
        public void RotateBlock()//Use this function in a rotate button
        {
            if (currentColour == 0)
            {
                DeleteCurrentBlock();
                DrawIBlock(BlockCoordX[0], BlockCoordY[0]);
                DrawBlock();
            }
            else if (currentColour == 1)
            {
                DeleteCurrentBlock();
                DrawLBlock(BlockCoordX[0], BlockCoordY[0]);
                DrawBlock();
            }
            else if (currentColour == 2)
            {
                DeleteCurrentBlock();
                DrawJBlock(BlockCoordX[0], BlockCoordY[0]);
                DrawBlock();
            }
            else if (currentColour == 3)
            {
                DeleteCurrentBlock();
                DrawSBlock(BlockCoordX[0], BlockCoordY[0]);
                DrawBlock();
            }
            else if (currentColour == 4)
            {
                DeleteCurrentBlock();
                DrawZBlock(BlockCoordX[0], BlockCoordY[0]);
                DrawBlock();
            }
            //No rotation for O block as it stays the same
            else if (currentColour == 6)
            {
                DeleteCurrentBlock();
                DrawTBlock(BlockCoordX[0], BlockCoordY[0]);
                DrawBlock();
            }

        }
        public char DisplayNextBlock()
        {
            if (nextSpawn == 0)
            {
                return 'I';
            }
            if (nextSpawn == 1)
            {
                return 'L';
            }
            if (nextSpawn == 2)
            {
                return 'J';
            }
            if (nextSpawn == 3)
            {
                return 'S';
            }
            if (nextSpawn == 4)
            {
                return 'Z';
            }
            if (nextSpawn == 5)
            {
                return 'O';
            }
            if (nextSpawn == 6)
            {
                return 'T';
            }
            else return 'E';

        }
        public void BlockSpawn()
        {
            rotationState = 0;
            BlockCoordX = new int[4];
            BlockCoordY = new int[4];
            int blockSpawn = spawn;
            //int blockSpawn = 6;
            if (blockSpawn == 0)
            {
                DrawIBlock(BlockStartX, BlockStartY);
                DrawBlock();
            }
            if (blockSpawn == 1)
            {
                DrawLBlock(BlockStartX, BlockStartY);
                DrawBlock();
            }
            if (blockSpawn == 2)
            {
                DrawJBlock(BlockStartX, BlockStartY);
                DrawBlock();
            }
            if (blockSpawn == 3)
            {
                DrawSBlock(BlockStartX, BlockStartY);
                DrawBlock();
            }
            if (blockSpawn == 4)
            {
                DrawZBlock(BlockStartX, BlockStartY);
                DrawBlock();
            }
            if (blockSpawn == 5)
            {
                DrawOBlock(BlockStartX, BlockStartY);
                DrawBlock();
            }
            if (blockSpawn == 6)
            {
                DrawTBlock(BlockStartX, BlockStartY);
                DrawBlock();
            }
            NextBlock();
        }
        void btnEvent_Click(object sender, EventArgs e)
        {
            Console.WriteLine(((Button)sender).Text);
        }

        void btnRotate_Click(object sender, EventArgs e)
        {
            RotateBlock();
        }

        void btnMove_Click(object sender, EventArgs e)
        {
            MoveBlockDown();
        }

        void btnLeft_Click(object sender, EventArgs e)
        {
            MoveBlockLeft();
        }

        void btnRight_Click(object sender, EventArgs e)
        {
            MoveBlockRight();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}