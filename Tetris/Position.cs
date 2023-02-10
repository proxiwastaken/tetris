using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
     public class Position
    {
        public int row { get; set; }
        public int column { get; set; }
        public Position(int x, int y) //The Class stores a row and a column
        {
           row = x;
           column = y;
        }
    }
}
