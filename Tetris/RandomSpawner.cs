using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class RandomSpawner
    {
        public int RandomNum()//Random Number Generator between 0-6 to decide the shape
        {
            Random random = new Random();
            int randomSpawn = random.Next(0, 6);
            return randomSpawn;
        }
        public void shapeSpawn(int random)//The number you get will choose which shape to spawn
        {
            if (random == 0)
            {

            }
            if (random == 1)
            {

            }
            if (random == 2)
            {

            }
            if (random == 3)
            {

            }
            if (random == 4)
            {

            }
            if (random == 5)
            {

            }
            if (random == 6)
            {

            }
        }

        public RandomSpawner()
        {
            int rdm = RandomNum();
            shapeSpawn(rdm);
        }
    }
}
