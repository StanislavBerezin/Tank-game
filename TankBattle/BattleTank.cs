using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class BattleTank
    {
        private int xCoord;
        private int yCoord;
        GenericPlayer tPlayer;
        TankModel tTank;
        Game tGame;
        int tPower;
        float tAngle;
        int tWeapon;
        private Bitmap tankBMP;
        int health;




        public BattleTank(GenericPlayer player, int tankX, int tankY, Game game)
        {
            //setting up the values we recieve to the variables in this class
            this.tPlayer = player;
            this.xCoord = tankX;
            this.yCoord = tankY;
            this.tGame = game;
            this.tPower = 25;
            this.tAngle = 0;
            this.tWeapon = 0;
            
            //creating a tank, getting color, setting angle and getting armour
            //seems like the order of things is very important
            //was failing tests coz of the null excpetion, took me couple of hours
            //to understand that it needs to be re-ordered.
            this.tTank = tPlayer.CreateTank();
            this.tankBMP = tTank.CreateBMP(tPlayer.GetTankColour(), this.tAngle);
            this.health = tTank.GetArmour();
            
            
            




        }

        public GenericPlayer GetPlayerById()
        {
            //returning the player
            return this.tPlayer;
        }

        public TankModel CreateTank()
        {
            //creating tank for the player
            return this.tPlayer.CreateTank();
        }

        public float GetPlayerAngle()
        {
            //getting angle of the player
            return this.tAngle;
        }

        public void Aim(float angle)
        {
            //getting aim of where the player is going to shoot and set it to the angle
            this.tAngle = angle;
        }

        public int GetPower()
        {
            //getting the power
            return this.tPower;
        }

        public void SetTankPower(int power)
        {
            //setting the power selected by player to power of tank
            this.tPower = power;
        }

        public int GetCurrentWeapon()
        {
            //getting the weapon
            return this.tWeapon;
        }
        public void SetWeaponIndex(int newWeapon)
        {
            //setting the chosen weapon to the weapon of tank
            this.tWeapon = newWeapon;
        }

        public void Paint(Graphics graphics, Size displaySize)
        {
            //copied from ams with slight changes
            //its drawing a tank
            int drawX1 = displaySize.Width * this.xCoord / Map.WIDTH;
            int drawY1 = displaySize.Height * this.yCoord / Map.HEIGHT;
            int drawX2 = displaySize.Width * (this.xCoord + TankModel.WIDTH) / Map.WIDTH;
            int drawY2 = displaySize.Height * (this.yCoord + TankModel.HEIGHT) / Map.HEIGHT;
            graphics.DrawImage(tankBMP, new Rectangle(drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1));

            int drawY3 = displaySize.Height * (this.yCoord - TankModel.HEIGHT) / Map.HEIGHT;
            Font font = new Font("Arial", 8);
            Brush brush = new SolidBrush(Color.White);

            int pct = this.health * 100 / tTank.GetArmour();
            if (pct < 100)
            {
                graphics.DrawString(pct + "%", font, brush, new Point(drawX1, drawY3));
            }
        }

        public int GetX()
        {
            //getting x coordinates of the tank
            return this.xCoord;
        }
        public int YPos()
        {
            //getting y coordinates
            return this.yCoord;
        }

        public void Fire()
        {
            //getting the fireweapn

            TankModel tank = CreateTank();
            tank.FireWeapon(this.tWeapon, this, this.tGame);
        }

        public void DamageArmour(int damageAmount)
        {
            ///the damage that was received decrements the health of the player
            this.health -= damageAmount;
        }

        public bool IsAlive()
        {

            //if health is less or = 0 set the game over => false(meaning dead) or otherwise if more than 0
            if(this.health <= 0)
            {
                return false;
            }else
            {
                return true;
            }
        }

        public bool GravityStep()
        {
            Map map = tGame.GetBattlefield();

            if (IsAlive() == false)
            {
                return false;
            }

            if (map.CheckTankCollide(xCoord, yCoord + 1))
            {
                return false;
            }

            yCoord++;
            health--;

            if (yCoord == Map.HEIGHT - TankModel.HEIGHT)
            {
                health = 0;
            }

            return true;
        }
    }
}
