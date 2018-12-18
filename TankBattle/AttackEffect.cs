using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public abstract class AttackEffect
    {
        protected Game game;

        public void RecordCurrentGame(Game game)
        {
            this.game = game;
        }

        public abstract void Tick();
        public abstract void Paint(Graphics graphics, Size displaySize);
    }
}
