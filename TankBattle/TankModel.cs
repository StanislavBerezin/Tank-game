using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public abstract class TankModel
    {
        public const int WIDTH = 4;
        public const int HEIGHT = 3;
        public const int NUM_TANKS = 1;

        public abstract int[,] DisplayTankSprite(float angle);

        public static void LineDraw(int[,] graphic, int X1, int Y1, int X2, int Y2)
        {
            if (X1 != X2)
            {
                if (X2 < X1) //switch X1 and X2 because for the next cicle I need to know which one is bigger
                {
                    int app = X1;
                    X1 = X2;
                    X2 = app;
                    //I also switch the Y because they are related to the respective X
                    app = Y1;
                    Y1 = Y2;
                    Y2 = app;
                }

                int dx = X2 - X1;
                int dy = Y2 - Y1;

                //this is the theorem suggested in the specifics
                for (int x = X1; x <= X2; x++)
                {
                    int y = Y1 + dy * (x - X1) / dx;
                    graphic[y, x] = 1;
                }
            }
            else //in this case X1 == X2 it means its a straight line so I only 
            {
                if (Y1 > Y2) //switch Y1 with Y2 because for the next cicle I need to know which one is bigger
                {
                    int app = Y1;
                    Y1 = Y2;
                    Y2 = app;
                }
                int x = X1; //or X2 because X1 == X2
                for (int y = Y1; y < Y2; y++) //move only the Y in a straight path
                {
                    graphic[y, x] = 1;
                }
            }
        }

        public Bitmap CreateBMP(Color tankColour, float angle)
        {
            int[,] tankGraphic = DisplayTankSprite(angle);
            int height = tankGraphic.GetLength(0);
            int width = tankGraphic.GetLength(1);

            Bitmap bmp = new Bitmap(width, height);
            Color transparent = Color.FromArgb(0, 0, 0, 0);
            Color tankOutline = Color.FromArgb(255, 0, 0, 0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tankGraphic[y, x] == 0)
                    {
                        bmp.SetPixel(x, y, transparent);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, tankColour);
                    }
                }
            }

            // Outline each pixel
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    if (tankGraphic[y, x] != 0)
                    {
                        if (tankGraphic[y - 1, x] == 0)
                            bmp.SetPixel(x, y - 1, tankOutline);
                        if (tankGraphic[y + 1, x] == 0)
                            bmp.SetPixel(x, y + 1, tankOutline);
                        if (tankGraphic[y, x - 1] == 0)
                            bmp.SetPixel(x - 1, y, tankOutline);
                        if (tankGraphic[y, x + 1] == 0)
                            bmp.SetPixel(x + 1, y, tankOutline);
                    }
                }
            }

            return bmp;
        }

        public abstract int GetArmour();

        public abstract string[] ListWeapons();

        public abstract void FireWeapon(int weapon, BattleTank playerTank, Game currentGame);

        public static TankModel CreateTank(int tankNumber)
        {
            return new MyTank();
        }
    }
}
