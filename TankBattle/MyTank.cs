using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    class MyTank : TankModel
    {
        public override int[,] DisplayTankSprite(float angle)
        {
            //int[12,16]
            int[,] tankShape = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
            { 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0},
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

            if (angle > -22.5 && angle < 22.5)
            {
                LineDraw(tankShape, 7, 6, 7, 1); //point to up
            }
            else if (angle > -67.5 && angle < -22.5)
            {
                LineDraw(tankShape, 7, 6, 3, 2); //point to middle left
            }
            else if (angle < -67.5)
            {
                LineDraw(tankShape, 7, 6, 2, 6); //point to low left
            }
            else if (angle > 22.5 && angle < 67.5)
            {
                LineDraw(tankShape, 7, 6, 1, 2); //point to middle right
            }
            else if (angle > 67.5)
            {
                LineDraw(tankShape, 7, 6, 12, 6); //point to low right
            }

            return tankShape;
        }

        public override void FireWeapon(int weapon, BattleTank playerTank, Game currentGame)
        {
            int x = playerTank.GetX();
            int y = playerTank.YPos();
            float xPos = (float)x + (TankModel.HEIGHT / 2);
            float yPos = (float)y + (TankModel.WIDTH / 2);
            GenericPlayer player = playerTank.GetPlayerById();
            Boom explosion = new Boom(100, 4, 4);
            Bullet projectile = new Bullet(xPos, yPos, playerTank.GetPlayerAngle(), playerTank.GetPower(), 0.01f, explosion, player);
            currentGame.AddEffect(projectile);
        }

        public override int GetArmour()
        {
            return 100;
        }

        public override string[] ListWeapons()
        {
            return new string[] { "Standard shell" };
        }
    }
}
