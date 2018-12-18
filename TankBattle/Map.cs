using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class Map
    {
        public const int WIDTH = 160;
        public const int HEIGHT = 120;
        private bool[,] battleGround;
        private Random rndNumber‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮ = new Random();

        public Map()
        {
            battleGround‎‫‭‌⁮‌⁬‬⁮‫⁫⁯⁯‫‪⁪‫⁫⁪‫‭‬‬‌⁮⁭⁯‌⁪⁬⁮‭⁪‌‭⁮⁮‏‪‮ = new bool[160, 120];
            int minHeight = 30;
            int maxHeight = 110;
            int establishHeight = rndNumber‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(minHeight, maxHeight);

            /// <summary>DoWork is a method in the TestClass class.
            /// 
            ///This method is broken into 3 parts, first we draw 1/3 of the map by -2, 3 
            /// Then the middle part of the map
            /// Then the last bit of the map
            /// 
            /// It is done to add some steep area inside of the game, to have different heights 
            /// It would add some cool effects and strategic advantages to some of the players if they get lucky
            /// 
            /// </summary>
            /// 
            ///THE FIRST PART

            //going all the width by one
            for (int widthByOne = 0; widthByOne < WIDTH/3; widthByOne++)
            {
                //here we incriment or decrement our height that will be used further
                do
                {
                    establishHeight += rndNumber‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(-1, 2);
                }
                //this will stop do while loop if we go beyond
                while (establishHeight <= minHeight || establishHeight >= maxHeight);

                ///here we use the established height that was made above
                for (int heightByOne = establishHeight; heightByOne < HEIGHT; heightByOne++)
                {
                    //now we plot it
                    this.battleGround‎‫‭‌⁮‌⁬‬⁮‫⁫⁯⁯‫‪⁪‫⁫⁪‫‭‬‬‌⁮⁭⁯‌⁪⁬⁮‭⁪‌[widthByOne, heightByOne] = true;
                }
                

            
               
            }

            ////SECOND PART

            //it has the same logic as the first one, however it starts from 53
            //then stops after 2/3 of the map
            //sometimes it it doesnt make steep area :(
            //but most of the time it does
            for (int widthByOne = 53; widthByOne < (WIDTH/3)*2; widthByOne++)
            {
                //here we decide if to increase or decrease to build a steep area in the middle
                do
                {
                    //if its getting close to the top then move it down
                    if(establishHeight < 50)
                    {
                        establishHeight += rndNumber‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(0, 3);
                    }
                    //otherwise move it up by decreasing established height
                    else if(establishHeight >= 60)
                    {
                        establishHeight += rndNumber‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(-3, 0);
                    }
                    else
                    {
                        establishHeight += rndNumber‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(-3, 0);
                    }
                    
                }
                while (establishHeight <= minHeight || establishHeight >= maxHeight);

                ///while we are doing width we establish height
                for (int heightByOne = establishHeight; heightByOne < HEIGHT; heightByOne++)
                {

                    this.battleGround‎‫‭‌⁮‌⁬‬⁮‫⁫⁯⁯‫‪⁪‫⁫⁪‫‭‬‬‌⁮⁭⁯‌⁪⁬⁮‭⁪‌[widthByOne, heightByOne] = true;
                }
            }

            ///THIRD PART
            /// exactly the same as the first part
            for (int widthByOne = 106; widthByOne < (0.2+WIDTH / 3) * 3; widthByOne++)
            {
                do
                {

                    establishHeight += rndNumber‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(-1, 2);
                }
                while (establishHeight <= minHeight || establishHeight >= maxHeight);

                ///while we are doing width we establish height
                for (int heightByOne = establishHeight; heightByOne < HEIGHT; heightByOne++)
                {

                    this.battleGround‎‫‭‌⁮‌⁬‬⁮‫⁫⁯⁯‫‪⁪‫⁫⁪‫‭‬‬‌⁮⁭⁯‌⁪⁬⁮‭⁪‌[widthByOne, heightByOne] = true;
                }
            }

        }

        public bool Get(int x, int y)
        {
            ///returning what was asked
            return battleGround[x, y];
        }

        public bool CheckTankCollide(int x, int y)
        {

            for (int horizontal = x; horizontal < x + TankModel.WIDTH; horizontal++)
            {
                for (int vertical = y; vertical < y + TankModel.HEIGHT; vertical++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < TankModel.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < TankModel.WIDTH; ix++)
                        {

                            if (Get(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                
                            return false;

                    }

                }
               
            }


            return true;
        }

        public int TankVerticalPosition(int x)
        {
            int lowestValidY = 0;
            for (int y = 0; y <= Map.HEIGHT - TankModel.HEIGHT; y++)
            {
                int colTiles = 0;
                for (int iy = 0; iy < TankModel.HEIGHT; iy++)
                {
                    for (int ix = 0; ix < TankModel.WIDTH; ix++)
                    {
                        if (Get(x + ix, y + iy))
                        {
                            colTiles++;
                        }
                    }
                }
                if (colTiles == 0)
                {
                    lowestValidY = y;
                }
            }

            return lowestValidY;
        }

        public void TerrainDestruction(float destroyX, float destroyY, float radius)
        {
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    float distance = (float)Math.Sqrt(Math.Pow(x - destroyX, 2) + Math.Pow(destroyY - destroyY, 2));
                    if (distance < radius)
                    {
                       battleGround[x, y] = false;
                    }
                }
            }
        }

        public bool GravityStep()
        {
            
                bool isMoved = false;

                for (int column = 0; column < WIDTH; column++)
                {
                    for (int row = HEIGHT - 2; row >= 0; row--)
                    {
                        if (battleGround[row + 1, column] == false && battleGround[row, column] == true)
                        {
                            battleGround[row, column] = true;
                            battleGround[row, column] = false;
                            isMoved = true;
                        }
                    }
                }
                if (isMoved)
                {
                    return true;
                }
                return false;
            
        }
    }
}
