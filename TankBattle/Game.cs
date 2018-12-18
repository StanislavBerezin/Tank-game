using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TankBattle
{
    public class Game
    {

        private int[] arrNumPlayer;
        private int[] arrNumRound;
        private int currentPlayer;
        private int currentRound;
        private int windSpeed;
        private int startingGenericPlayer;
        private GenericPlayer[] modelPlayerController;
        private BattleTank[] modelPlayerTank;
        private Map drawBattleGround;
        private List<AttackEffect> attackEffectList;
      

        public Game(int numPlayers, int numRounds)
        {
            /// <summary> Game method that used to initilise the game, recieves number of players and numRounds
            /// 
            /// IF the amount of players is more than 8 we set the array = to 8
            /// If the its less than 2 then we set it to 2
            /// The same logic applies for num round but for numbers 100 and 1
            /// 
            /// </summary>


            //if numPlayer more than 8
            if (numPlayers > 8)
            {
                numPlayers = 8;
                //setting the array
                this.arrNumPlayer = new int[numPlayers];

            }
            //if its less than 2
            else if (numPlayers < 2)
            {
                numPlayers = 2;
                //setting the array
                this.arrNumPlayer = new int[numPlayers];

            }
            //if number of rounds more than 100
            else if (numRounds > 100)
            {
                numRounds = 100;
                //we set the array to 100
                this.arrNumRound = new int[numRounds];
            }
            //if number of rounds less than 1
            else if (numRounds < 1)
            {
                
                numRounds = 1;
                //setting the array
                this.arrNumRound = new int[numRounds];
            }
            // if it was any number in between those 2
            else
            {
                //then we just set the arrays
                this.arrNumPlayer = new int[numPlayers];
                this.arrNumRound = new int[numRounds];
            }
            //when we have initialised array then we setup the models for the players and tank
            //accordignly to the length of those arrays
            this.modelPlayerController = new GenericPlayer[this.arrNumPlayer.Length];
            this.modelPlayerTank = new BattleTank[this.arrNumPlayer.Length];
        }


        //returning the number of players if called outside of this class
        public int PlayerCount()
        {
            
            return arrNumPlayer.Length;
        }

        //returning the number of rounds if called outside of this class
        public int GetCurrentRound()
        {
            return currentRound;
        }

        //count the number of rounds and return it
        public int GetRounds()
        {
            int roundsCount = arrNumRound.Length;
            return roundsCount;
        }

        //setting the player model based on the number recievd -1 because the array starts from 0
        public void SetPlayer(int playerNum, GenericPlayer player)
        {
            
            this.modelPlayerController[playerNum - 1] = player;
        }

        //returning the player by its number, do -1 because array starts from 0
        public GenericPlayer GetPlayerById(int playerNum)
        {
            return this.modelPlayerController[playerNum - 1];
        }

        //returning the tank model for the player
        public BattleTank GetGameplayTank(int playerNum)
        {
            return this.modelPlayerTank[playerNum - 1];
        }

        //returning the colors based on number recieved
        public static Color GetTankColour(int playerNum)
        {
            Color c = Color.Blue;
            Color c1 = Color.Red;
            Color c2 = Color.Aqua;
            Color c3 = Color.LightGreen;
            Color c4 = Color.Pink;
            Color c5 = Color.Chartreuse;
            Color c6 = Color.Purple;
            Color c7 = Color.Orange;

            if (playerNum == 1)
            {
                return c1;
            }
            else if (playerNum == 2)
            {
                return c2;
            }
            else if (playerNum == 3)
            {
                return c3;
            }
            else if (playerNum == 4)
            {
                return c4;
            }
            else if (playerNum == 5)
            {
                return c5;
            }
            else if (playerNum == 6)
            {
                return c6;
            }
            else if (playerNum == 7)
            {
                return c7;
            }
            else
            {
                return c;
            }

        }

        //calculating x positions of players
        public static int[] CalculatePlayerPositions(int numPlayers)
        {
            //setting up the array
            int[] playerPos = new int[numPlayers];

            //getting the split distance between the amount of players
            float splitDistance = 160 / numPlayers;

            //converting float to int into another variable
            int initialDist = Convert.ToInt32(Math.Round(splitDistance));

            //dividing the result by the number of players again
            int dividedDist = Convert.ToInt32(Math.Round(splitDistance / numPlayers));

            for (int player = 0; player < numPlayers; player++)
            {
                //assigning a position to each indexed player
                playerPos[player] = dividedDist;

                //increasing the divided dist
                dividedDist = dividedDist + initialDist;
            }
            //returning the array itself
            return playerPos;



        }
        //randomising array
        public static void Randomise(int[] array)
        {
            int current;
            int change;
            Random rndNumber = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                //getting a random number within the array length
                current = rndNumber.Next(array.Length);
                //changer is equal to the pos of array
                change = array[i];
                //pos of array eqauls to the new random number
                array[i] = array[current];
                //array at index of random number equals to change
                array[current] = change;
            }
        }

        //initialising the game
        //current round should be at
        //initiliasing a function for newRound()
        public void CommenceGame()
        {
            this.currentRound = 1;
            this.startingGenericPlayer = 0;
            NewRound();
        }

        public void NewRound()
        {
           
            this.currentPlayer = this.startingGenericPlayer;
            //initialising map
            this.drawBattleGround = new Map();
           
            ///making an array to store the positions of players
            int[] positionPlayers = new int[arrNumPlayer.Length];
            positionPlayers = CalculatePlayerPositions(arrNumPlayer.Length);

            //random positioning for players
            Randomise(positionPlayers);
            
            //player to keep start looping through the player
            int player = 0;

            do
            {
                //if null means its all matching and no need to create anymore players
                //so we break it
                if (modelPlayerController[player] == null)
                {
                    break;
                }
                //if not null then keep making each player
                else
                {
                    //for each player to start the round
                    modelPlayerController[player].StartRound();
                    //taking the horizontal position from positionPlayer array at the current index
                    int tHorizontalPosition = positionPlayers[player];

                    //getting positioning for tank from map class
                    int tVerticalPosition = drawBattleGround.TankVerticalPosition(tHorizontalPosition);

                    //create the tank itself with values that we gathered from above
                    BattleTank tank = new BattleTank(modelPlayerController[player], tHorizontalPosition, tVerticalPosition, this);

                    //each instance that we are looping through creates a tank
                    modelPlayerTank[player] = tank;
                    player++;
                }

            } while (player < this.arrNumPlayer.Length);

            //random number, wind speed, initialising form
            Random rnd = new Random();
            this.windSpeed = rnd.Next(-100, 100);
            GameForm newForm = new GameForm(this);
            newForm.Show();
            
        }

        //returning the battleGround
        public Map GetBattlefield()
        {
            return this.drawBattleGround;
        }

        public void DisplayTanks(Graphics graphics, Size displaySize)
        {

            foreach (BattleTank pt in modelPlayerTank)
            {
                if (pt != null)
                {
                    if (pt.IsAlive())
                    {
                        pt.Paint(graphics, displaySize);
                    }
                }
            }


        }

        public BattleTank CurrentPlayerTank()
        {
            return modelPlayerTank[currentPlayer];
        }

        public void AddEffect(AttackEffect weaponEffect)
        {
            weaponEffect.RecordCurrentGame(this);
            attackEffectList.Add(weaponEffect);
        }

        public bool ProcessWeaponEffects()
        {
            foreach (AttackEffect attackEffect in attackEffectList)
            {
                attackEffect.Tick();
            }

            return true;
        }

        public void RenderEffects(Graphics graphics, Size displaySize)
        {
           
        }

        public void CancelEffect(AttackEffect weaponEffect)
        {
            attackEffectList.Remove(weaponEffect);
        }

        public bool DetectCollision(float projectileX, float projectileY)
        {

            if (projectileX < 0 || projectileY < 0 || projectileX > Map.HEIGHT || projectileY > Map.WIDTH)
            {
                return false;
            }

            //projectile hit terrain
            if (drawBattleGround.Get((int)projectileX, (int)projectileY))
            {
                return true;
            }

            //projectile hit a tank
            foreach (BattleTank player in modelPlayerTank)
            {
                if (player != null)
                {
                    if ((projectileX <= (player.GetX() + TankModel.WIDTH / 2) && projectileX >= (player.GetX() - TankModel.WIDTH / 2)) &&
                        (projectileY <= (player.YPos() + TankModel.HEIGHT / 2) && projectileY >= (player.YPos() - TankModel.HEIGHT / 2)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void DamageArmour(float damageX, float damageY, float explosionDamage, float radius)
        {
            throw new NotImplementedException();
        }

        public bool GravityStep()
        {
            throw new NotImplementedException();
        }

        public bool FinaliseTurn()
        {
            throw new NotImplementedException();
        }

        public void CheckWinner()
        {
            throw new NotImplementedException();
        }

        public void NextRound()
        {
            if ((currentRound + 1) <= arrNumRound.Length)
            {
                currentRound++;

                if ((currentPlayer + 1) > arrNumPlayer.Length)
                {
                    currentPlayer = 0;
                }
                else
                {
                    currentPlayer++;
                }

                NewRound();
            }
            else
            {
                IntroForm form = new IntroForm();
                form.Show();
            }
        }
        
        public int WindSpeed()
        {
            return this.windSpeed;
        }
    }
}
