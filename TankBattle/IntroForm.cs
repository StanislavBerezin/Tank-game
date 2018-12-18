using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class IntroForm : Form
    {
        public IntroForm()
        {
            InitializeComponent();


        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            Game game = new Game(3, 3);
            GenericPlayer player1 = new PlayerController("Player 1", TankModel.CreateTank(1), Game.GetTankColour(1));
            GenericPlayer player2 = new PlayerController("Player 2", TankModel.CreateTank(1), Game.GetTankColour(2));
            GenericPlayer player3 = new PlayerController("qr2", TankModel.CreateTank(1), Game.GetTankColour(3));
            
        
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);
            game.SetPlayer(3, player3);
           
          
            game.CommenceGame();
        }
    }
}
