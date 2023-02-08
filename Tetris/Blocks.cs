using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Blocks
    {

        protected  Position StartingOffset { get; }// Offset of the block off bounds at the start
        protected Position[][] ShapeTile { get; } //2D Array which contains the positions of the block in rotation states
        public abstract int BlockID { get; }//ID of the block

        private Position offset;// Offset of the block off bounds
        private int Rotation;//
        
        
        public Blocks()
        {
            offset = new Position(StartingOffset.row, StartingOffset.column);
        }

        public IEnumerable<Position> TileP()
        {
            foreach (Position p in ShapeTile[Rotation])
            {
                yield return new Position(p.row + offset.row, p.column + offset.column);
            }

        }

        public void RotateCW()
        {
            if (Rotation != 3)
            {
                Rotation++;
            }
            else
            {
                Rotation= 0;
            }
        }
        public void RotateCCW() 
        {
            if(Rotation != 0)
            {
                Rotation--;
            }
            else
            {
                Rotation = 3;
            }
        
        }
    }
}
