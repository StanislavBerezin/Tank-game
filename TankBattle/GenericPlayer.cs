using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    abstract public class GenericPlayer
    {
        
       //setting up a string for player
        private string playerName;
        //setting up an int to see how many rounds won
        private int hasWon;
        //collor for player
        private Color playerColour;
        //creating a playerTank from TankModel class
        private TankModel playerTank;


        public GenericPlayer(string name, TankModel tank, Color colour)
        {
            //making sure when generic player is called it will have a color, name, and tankmodel
            //also checking how many times it has won
            this.hasWon = 0;
            this.playerTank = tank;
            this.playerColour = colour;
            this.playerName = name;
            
        }
        //returning the tank
        public TankModel CreateTank()
        {
            return this.playerTank;
        }
        //returning the name
        public string Identifier()
        {
            return playerName;
        }
        //returning the color
        public Color GetTankColour()
        {
            return playerColour;
        }

        //incrimenting by 1 whenever is called 
        public void Winner()
        {
            this.hasWon++;
        }
        //returning how many times has Won
        public int GetWins()
        {
            return this.hasWon;
        }
        //Abstract classes
        public abstract void StartRound();

        public abstract void BeginTurn(GameForm gameplayForm, Game currentGame);

        public abstract void HitPos(float x, float y);
    }
}
