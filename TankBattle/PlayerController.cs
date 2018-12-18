using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public class PlayerController : GenericPlayer
    {

        private TankModel tank;
        private string name;
        private Color colour;

        public PlayerController(string name, TankModel tank, Color colour) : base(name, tank, colour)
        {
            this.tank = tank;
            this.name = name;
            this.colour = colour;
        }

        public override void StartRound()
        {
            
        }

        public override void BeginTurn(GameForm gameplayForm, Game currentGame)
        {
            gameplayForm.EnableTankControls();
        }

        public override void HitPos(float x, float y)
        {
            
        }
    }
}
