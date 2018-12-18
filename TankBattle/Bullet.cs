using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public class Bullet : AttackEffect
    {

        private float x;
        private float y;
        private float xVelocity;
        private float yVelocity;
        private float angle;
        private float power;
        private float gravity;
        private Boom explosion;
        private GenericPlayer player;


        public Bullet(float x, float y, float angle, float power, float gravity, Boom explosion, GenericPlayer player)
        {
            this.angle = angle;
            this.power = power;
            this.gravity = gravity;
            this.explosion = explosion;
            this.player = player;
            this.x = x;
            this.y = y;

            float angleRadiant = (90 - angle) * (float)Math.PI / 180;
            float magnitude = power / 50;

            this.xVelocity = (float)Math.Cos(angleRadiant) * magnitude;
            this.yVelocity = (float)Math.Sin(angleRadiant) * magnitude;
        }

        public override void Tick()
        {
            for (int i = 0; i < 10; i++)
            {
                x += xVelocity;
                y += yVelocity;
                x += game.WindSpeed() / 1000.0f;

                if (x > Map.HEIGHT || y > Map.WIDTH)
                {
                    game.CancelEffect(this);
                }
                else if (game.DetectCollision(x, y))
                {
                    player.HitPos(x, y);
                    explosion.Explode(x, y);
                    game.AddEffect(explosion);
                    game.CancelEffect(this);
                    return;
                }

                yVelocity += gravity;
            }
        }

        public override void Paint(Graphics graphics, Size size)
        {
            float x = (float)this.x * size.Width / Map.WIDTH;
            float y = (float)this.y * size.Height / Map.HEIGHT;
            float s = size.Width / Map.WIDTH;

            RectangleF r = new RectangleF(x - s / 2.0f, y - s / 2.0f, s, s);
            Brush b = new SolidBrush(Color.WhiteSmoke);

            graphics.FillEllipse(b, r);
        }
    }
}
