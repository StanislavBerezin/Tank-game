using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TankBattle;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TankBattleTestSuite
{
    class RequirementException : Exception
    {
        public RequirementException()
        {
        }

        public RequirementException(string message) : base(message)
        {
        }

        public RequirementException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    class Test
    {
        #region Testing Code

        private delegate bool TestCase();

        private static string ErrorDescription = null;

        private static void SetErrorDescription(string desc)
        {
            ErrorDescription = desc;
        }

        private static bool FloatEquals(float a, float b)
        {
            if (Math.Abs(a - b) < 0.01) return true;
            return false;
        }

        private static Dictionary<string, string> unitTestResults = new Dictionary<string, string>();

        private static void Passed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[passed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                throw new Exception("ErrorDescription found for passing test case");
            }
            Console.WriteLine();
        }
        private static void Failed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[failed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                Console.Write("\n{0}", ErrorDescription);
                ErrorDescription = null;
            }
            Console.WriteLine();
        }
        private static void FailedToMeetRequirement(string name, string comment)
        {
            Console.Write("[      ] ");
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("{0}", comment);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        private static void DoTest(TestCase test)
        {
            // Have we already completed this test?
            if (unitTestResults.ContainsKey(test.Method.ToString()))
            {
                return;
            }

            bool passed = false;
            bool metRequirement = true;
            string exception = "";
            try
            {
                passed = test();
            }
            catch (RequirementException e)
            {
                metRequirement = false;
                exception = e.Message;
            }
            catch (Exception e)
            {
                exception = e.GetType().ToString();
            }

            string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
            string fnName = test.Method.ToString().Split('0')[1];

            if (metRequirement)
            {
                if (passed)
                {
                    unitTestResults[test.Method.ToString()] = "Passed";
                    Passed(string.Format("{0}.{1}", className, fnName), exception);
                }
                else
                {
                    unitTestResults[test.Method.ToString()] = "Failed";
                    Failed(string.Format("{0}.{1}", className, fnName), exception);
                }
            }
            else
            {
                unitTestResults[test.Method.ToString()] = "Failed";
                FailedToMeetRequirement(string.Format("{0}.{1}", className, fnName), exception);
            }
            Cleanup();
        }

        private static Stack<string> errorDescriptionStack = new Stack<string>();


        private static void Requires(TestCase test)
        {
            string result;
            bool wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

            if (!wasTested)
            {
                // Push the error description onto the stack (only thing that can change, not that it should)
                errorDescriptionStack.Push(ErrorDescription);

                // Do the test
                DoTest(test);

                // Pop the description off
                ErrorDescription = errorDescriptionStack.Pop();

                // Get the proper result for out
                wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

                if (!wasTested)
                {
                    throw new Exception("This should never happen");
                }
            }

            if (result == "Failed")
            {
                string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
                string fnName = test.Method.ToString().Split('0')[1];

                throw new RequirementException(string.Format("-> {0}.{1}", className, fnName));
            }
            else if (result == "Passed")
            {
                return;
            }
            else
            {
                throw new Exception("This should never happen");
            }

        }

        #endregion

        #region Test Cases
        private static Game InitialiseGame()
        {
            Requires(TestGame0Game);
            Requires(TestTankModel0CreateTank);
            Requires(TestGenericPlayer0PlayerController);
            Requires(TestGame0SetPlayer);

            Game game = new Game(2, 1);
            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer player1 = new PlayerController("player1", tank, Color.Orange);
            GenericPlayer player2 = new PlayerController("player2", tank, Color.Purple);
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);
            return game;
        }
        private static void Cleanup()
        {
            while (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Dispose();
            }
        }
        private static bool TestGame0Game()
        {
            Game game = new Game(2, 1);
            return true;
        }
        private static bool TestGame0PlayerCount()
        {
            Requires(TestGame0Game);

            Game game = new Game(2, 1);
            return game.PlayerCount() == 2;
        }
        private static bool TestGame0GetRounds()
        {
            Requires(TestGame0Game);

            Game game = new Game(3, 5);
            return game.GetRounds() == 5;
        }
        private static bool TestGame0SetPlayer()
        {
            Requires(TestGame0Game);
            Requires(TestTankModel0CreateTank);

            Game game = new Game(2, 1);
            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer player = new PlayerController("playerName", tank, Color.Orange);
            game.SetPlayer(1, player);
            return true;
        }
        private static bool TestGame0GetPlayerById()
        {
            Requires(TestGame0Game);
            Requires(TestTankModel0CreateTank);
            Requires(TestGenericPlayer0PlayerController);

            Game game = new Game(2, 1);
            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer player = new PlayerController("playerName", tank, Color.Orange);
            game.SetPlayer(1, player);
            return game.GetPlayerById(1) == player;
        }
        private static bool TestGame0GetTankColour()
        {
            Color[] arrayOfColours = new Color[8];
            for (int i = 0; i < 8; i++)
            {
                arrayOfColours[i] = Game.GetTankColour(i + 1);
                for (int j = 0; j < i; j++)
                {
                    if (arrayOfColours[j] == arrayOfColours[i]) return false;
                }
            }
            return true;
        }
        private static bool TestGame0CalculatePlayerPositions()
        {
            int[] positions = Game.CalculatePlayerPositions(8);
            for (int i = 0; i < 8; i++)
            {
                if (positions[i] < 0) return false;
                if (positions[i] > 160) return false;
                for (int j = 0; j < i; j++)
                {
                    if (positions[j] == positions[i]) return false;
                }
            }
            return true;
        }
        private static bool TestGame0Randomise()
        {
            int[] ar = new int[100];
            for (int i = 0; i < 100; i++)
            {
                ar[i] = i;
            }
            Game.Randomise(ar);
            for (int i = 0; i < 100; i++)
            {
                if (ar[i] != i)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestGame0CommenceGame()
        {
            Game game = InitialiseGame();
            game.CommenceGame();

            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestGame0GetBattlefield()
        {
            Requires(TestMap0Map);
            Game game = InitialiseGame();
            game.CommenceGame();
            Map battlefield = game.GetBattlefield();
            if (battlefield != null) return true;

            return false;
        }
        private static bool TestGame0CurrentPlayerTank()
        {
            Requires(TestGame0Game);
            Requires(TestTankModel0CreateTank);
            Requires(TestGenericPlayer0PlayerController);
            Requires(TestGame0SetPlayer);
            Requires(TestBattleTank0GetPlayerById);

            Game game = new Game(2, 1);
            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer player1 = new PlayerController("player1", tank, Color.Orange);
            GenericPlayer player2 = new PlayerController("player2", tank, Color.Purple);
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);

            game.CommenceGame();
            BattleTank ptank = game.CurrentPlayerTank();
            if (ptank.GetPlayerById() != player1 && ptank.GetPlayerById() != player2)
            {
                return false;
            }
            if (ptank.CreateTank() != tank)
            {
                return false;
            }

            return true;
        }

        private static bool TestTankModel0CreateTank()
        {
            TankModel tank = TankModel.CreateTank(1);
            if (tank != null) return true;
            else return false;
        }
        private static bool TestTankModel0DisplayTankSprite()
        {
            Requires(TestTankModel0CreateTank);
            TankModel tank = TankModel.CreateTank(1);

            int[,] tankGraphic = tank.DisplayTankSprite(45);
            if (tankGraphic.GetLength(0) != 12) return false;
            if (tankGraphic.GetLength(1) != 16) return false;
            // We don't really care what the tank looks like, but the 45 degree tank
            // should at least look different to the -45 degree tank
            int[,] tankGraphic2 = tank.DisplayTankSprite(-45);
            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (tankGraphic2[y, x] != tankGraphic[y, x])
                    {
                        return true;
                    }
                }
            }

            SetErrorDescription("Tank with turret at -45 degrees looks the same as tank with turret at 45 degrees");

            return false;
        }
        private static void DisplayLine(int[,] array)
        {
            string report = "";
            report += "A line drawn from 3,0 to 0,3 on a 4x4 array should look like this:\n";
            report += "0001\n";
            report += "0010\n";
            report += "0100\n";
            report += "1000\n";
            report += "The one produced by TankModel.LineDraw() looks like this:\n";
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    report += array[y, x] == 1 ? "1" : "0";
                }
                report += "\n";
            }
            SetErrorDescription(report);
        }
        private static bool TestTankModel0LineDraw()
        {
            int[,] ar = new int[,] { { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 } };
            TankModel.LineDraw(ar, 3, 0, 0, 3);

            // Ideally, the line we want to see here is:
            // 0001
            // 0010
            // 0100
            // 1000

            // However, as we aren't that picky, as long as they have a 1 in every row and column
            // and nothing in the top-left and bottom-right corners

            int[] rows = new int[4];
            int[] cols = new int[4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (ar[y, x] == 1)
                    {
                        rows[y] = 1;
                        cols[x] = 1;
                    }
                    else if (ar[y, x] > 1 || ar[y, x] < 0)
                    {
                        // Only values 0 and 1 are permitted
                        SetErrorDescription(string.Format("Somehow the number {0} got into the array.", ar[y, x]));
                        return false;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (rows[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
                if (cols[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
            }
            if (ar[0, 0] == 1)
            {
                DisplayLine(ar);
                return false;
            }
            if (ar[3, 3] == 1)
            {
                DisplayLine(ar);
                return false;
            }

            return true;
        }
        private static bool TestTankModel0GetArmour()
        {
            Requires(TestTankModel0CreateTank);
            // As long as it's > 0 we're happy
            TankModel tank = TankModel.CreateTank(1);
            if (tank.GetArmour() > 0) return true;
            return false;
        }
        private static bool TestTankModel0ListWeapons()
        {
            Requires(TestTankModel0CreateTank);
            // As long as there's at least one result and it's not null / a blank string, we're happy
            TankModel tank = TankModel.CreateTank(1);
            if (tank.ListWeapons().Length == 0) return false;
            if (tank.ListWeapons()[0] == null) return false;
            if (tank.ListWeapons()[0] == "") return false;
            return true;
        }

        private static GenericPlayer CreateTestingPlayer()
        {
            Requires(TestTankModel0CreateTank);
            Requires(TestGenericPlayer0PlayerController);

            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer player = new PlayerController("player1", tank, Color.Aquamarine);
            return player;
        }

        private static bool TestGenericPlayer0PlayerController()
        {
            Requires(TestTankModel0CreateTank);

            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer player = new PlayerController("player1", tank, Color.Aquamarine);
            if (player != null) return true;
            return false;
        }
        private static bool TestGenericPlayer0CreateTank()
        {
            Requires(TestTankModel0CreateTank);
            Requires(TestGenericPlayer0PlayerController);

            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer p = new PlayerController("player1", tank, Color.Aquamarine);
            if (p.CreateTank() == tank) return true;
            return false;
        }
        private static bool TestGenericPlayer0Identifier()
        {
            Requires(TestTankModel0CreateTank);
            Requires(TestGenericPlayer0PlayerController);

            const string PLAYER_NAME = "kfdsahskfdajh";
            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer p = new PlayerController(PLAYER_NAME, tank, Color.Aquamarine);
            if (p.Identifier() == PLAYER_NAME) return true;
            return false;
        }
        private static bool TestGenericPlayer0GetTankColour()
        {
            Requires(TestTankModel0CreateTank);
            Requires(TestGenericPlayer0PlayerController);

            Color playerColour = Color.Chartreuse;
            TankModel tank = TankModel.CreateTank(1);
            GenericPlayer p = new PlayerController("player1", tank, playerColour);
            if (p.GetTankColour() == playerColour) return true;
            return false;
        }
        private static bool TestGenericPlayer0Winner()
        {
            GenericPlayer p = CreateTestingPlayer();
            p.Winner();
            return true;
        }
        private static bool TestGenericPlayer0GetWins()
        {
            Requires(TestGenericPlayer0Winner);

            GenericPlayer p = CreateTestingPlayer();
            int wins = p.GetWins();
            p.Winner();
            if (p.GetWins() == wins + 1) return true;
            return false;
        }
        private static bool TestPlayerController0StartRound()
        {
            GenericPlayer p = CreateTestingPlayer();
            p.StartRound();
            return true;
        }
        private static bool TestPlayerController0BeginTurn()
        {
            Requires(TestGame0CommenceGame);
            Requires(TestGame0GetPlayerById);
            Game game = InitialiseGame();

            game.CommenceGame();

            // Find the gameplay form
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Game.CommenceGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in GameForm");
                return false;
            }

            // Disable the control panel to check that NewTurn enables it
            controlPanel.Enabled = false;

            game.GetPlayerById(1).BeginTurn(gameplayForm, game);

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after HumanPlayer.NewTurn()");
                return false;
            }
            return true;

        }
        private static bool TestPlayerController0HitPos()
        {
            GenericPlayer p = CreateTestingPlayer();
            p.HitPos(0, 0);
            return true;
        }

        private static bool TestBattleTank0BattleTank()
        {
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            return true;
        }
        private static bool TestBattleTank0GetPlayerById()
        {
            Requires(TestBattleTank0BattleTank);
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            if (playerTank.GetPlayerById() == p) return true;
            return false;
        }
        private static bool TestBattleTank0CreateTank()
        {
            Requires(TestBattleTank0BattleTank);
            Requires(TestGenericPlayer0CreateTank);
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            if (playerTank.CreateTank() == playerTank.GetPlayerById().CreateTank()) return true;
            return false;
        }
        private static bool TestBattleTank0GetPlayerAngle()
        {
            Requires(TestBattleTank0BattleTank);
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            float angle = playerTank.GetPlayerAngle();
            if (angle >= -90 && angle <= 90) return true;
            return false;
        }
        private static bool TestBattleTank0Aim()
        {
            Requires(TestBattleTank0BattleTank);
            Requires(TestBattleTank0GetPlayerAngle);
            float angle = 75;
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            playerTank.Aim(angle);
            if (FloatEquals(playerTank.GetPlayerAngle(), angle)) return true;
            return false;
        }
        private static bool TestBattleTank0GetPower()
        {
            Requires(TestBattleTank0BattleTank);
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);

            playerTank.GetPower();
            return true;
        }
        private static bool TestBattleTank0SetTankPower()
        {
            Requires(TestBattleTank0BattleTank);
            Requires(TestBattleTank0GetPower);
            int power = 65;
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            playerTank.SetTankPower(power);
            if (playerTank.GetPower() == power) return true;
            return false;
        }
        private static bool TestBattleTank0GetCurrentWeapon()
        {
            Requires(TestBattleTank0BattleTank);

            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);

            playerTank.GetCurrentWeapon();
            return true;
        }
        private static bool TestBattleTank0SetWeaponIndex()
        {
            Requires(TestBattleTank0BattleTank);
            Requires(TestBattleTank0GetCurrentWeapon);
            int weapon = 3;
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            playerTank.SetWeaponIndex(weapon);
            if (playerTank.GetCurrentWeapon() == weapon) return true;
            return false;
        }
        private static bool TestBattleTank0Paint()
        {
            Requires(TestBattleTank0BattleTank);
            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            playerTank.Paint(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestBattleTank0GetX()
        {
            Requires(TestBattleTank0BattleTank);

            GenericPlayer p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, x, y, game);
            if (playerTank.GetX() == x) return true;
            return false;
        }
        private static bool TestBattleTank0YPos()
        {
            Requires(TestBattleTank0BattleTank);

            GenericPlayer p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, x, y, game);
            if (playerTank.YPos() == y) return true;
            return false;
        }
        private static bool TestBattleTank0Fire()
        {
            Requires(TestBattleTank0BattleTank);

            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            playerTank.Fire();
            return true;
        }
        private static bool TestBattleTank0DamageArmour()
        {
            Requires(TestBattleTank0BattleTank);
            GenericPlayer p = CreateTestingPlayer();

            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            playerTank.DamageArmour(10);
            return true;
        }
        private static bool TestBattleTank0IsAlive()
        {
            Requires(TestBattleTank0BattleTank);
            Requires(TestBattleTank0DamageArmour);

            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            BattleTank playerTank = new BattleTank(p, 32, 32, game);
            if (!playerTank.IsAlive()) return false;
            playerTank.DamageArmour(playerTank.CreateTank().GetArmour());
            if (playerTank.IsAlive()) return false;
            return true;
        }
        private static bool TestBattleTank0GravityStep()
        {
            Requires(TestGame0GetBattlefield);
            Requires(TestMap0TerrainDestruction);
            Requires(TestBattleTank0BattleTank);
            Requires(TestBattleTank0DamageArmour);
            Requires(TestBattleTank0IsAlive);
            Requires(TestBattleTank0CreateTank);
            Requires(TestTankModel0GetArmour);

            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            game.CommenceGame();
            // Unfortunately we need to rely on DestroyTerrain() to get rid of any terrain that may be in the way
            game.GetBattlefield().TerrainDestruction(Map.WIDTH / 2.0f, Map.HEIGHT / 2.0f, 20);
            BattleTank playerTank = new BattleTank(p, Map.WIDTH / 2, Map.HEIGHT / 2, game);
            int oldX = playerTank.GetX();
            int oldY = playerTank.YPos();

            playerTank.GravityStep();

            if (playerTank.GetX() != oldX)
            {
                SetErrorDescription("Caused X coordinate to change.");
                return false;
            }
            if (playerTank.YPos() != oldY + 1)
            {
                SetErrorDescription("Did not cause Y coordinate to increase by 1.");
                return false;
            }

            int initialArmour = playerTank.CreateTank().GetArmour();
            // The tank should have lost 1 armour from falling 1 tile already, so do
            // (initialArmour - 2) damage to the tank then drop it again. That should kill it.

            if (!playerTank.IsAlive())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.DamageArmour(initialArmour - 2);
            if (!playerTank.IsAlive())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.GravityStep();
            if (playerTank.IsAlive())
            {
                SetErrorDescription("Tank survived despite taking enough falling damage to destroy it");
                return false;
            }

            return true;
        }
        private static bool TestMap0Map()
        {
            Map battlefield = new Map();
            return true;
        }
        private static bool TestMap0Get()
        {
            Requires(TestMap0Map);

            bool foundTrue = false;
            bool foundFalse = false;
            Map battlefield = new Map();
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.Get(x, y))
                    {
                        foundTrue = true;
                    }
                    else
                    {
                        foundFalse = true;
                    }
                }
            }

            if (!foundTrue)
            {
                SetErrorDescription("IsTileAt() did not return true for any tile.");
                return false;
            }

            if (!foundFalse)
            {
                SetErrorDescription("IsTileAt() did not return false for any tile.");
                return false;
            }

            return true;
        }
        private static bool TestMap0CheckTankCollide()
        {
            Requires(TestMap0Map);
            Requires(TestMap0Get);

            Map battlefield = new Map();
            for (int y = 0; y <= Map.HEIGHT - TankModel.HEIGHT; y++)
            {
                for (int x = 0; x <= Map.WIDTH - TankModel.WIDTH; x++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < TankModel.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < TankModel.WIDTH; ix++)
                        {

                            if (battlefield.Get(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        if (battlefield.CheckTankCollide(x, y))
                        {
                            SetErrorDescription("Found collision where there shouldn't be one");
                            return false;
                        }
                    }
                    else
                    {
                        if (!battlefield.CheckTankCollide(x, y))
                        {
                            SetErrorDescription("Didn't find collision where there should be one");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private static bool TestMap0TankVerticalPosition()
        {
            Requires(TestMap0Map);
            Requires(TestMap0Get);

            Map battlefield = new Map();
            for (int x = 0; x <= Map.WIDTH - TankModel.WIDTH; x++)
            {
                int lowestValid = 0;
                for (int y = 0; y <= Map.HEIGHT - TankModel.HEIGHT; y++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < TankModel.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < TankModel.WIDTH; ix++)
                        {

                            if (battlefield.Get(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        lowestValid = y;
                    }
                }

                int placedY = battlefield.TankVerticalPosition(x);
                if (placedY != lowestValid)
                {
                    SetErrorDescription(string.Format("Tank was placed at {0},{1} when it should have been placed at {0},{2}", x, placedY, lowestValid));
                    return false;
                }
            }
            return true;
        }
        private static bool TestMap0TerrainDestruction()
        {
            Requires(TestMap0Map);
            Requires(TestMap0Get);

            Map battlefield = new Map();
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.Get(x, y))
                    {
                        battlefield.TerrainDestruction(x, y, 0.5f);
                        if (battlefield.Get(x, y))
                        {
                            SetErrorDescription("Attempted to destroy terrain but it still exists");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            SetErrorDescription("Did not find any terrain to destroy");
            return false;
        }
        private static bool TestMap0GravityStep()
        {
            Requires(TestMap0Map);
            Requires(TestMap0Get);
            Requires(TestMap0TerrainDestruction);

            Map battlefield = new Map();
            for (int x = 0; x < Map.WIDTH; x++)
            {
                if (battlefield.Get(x, Map.HEIGHT - 1))
                {
                    if (battlefield.Get(x, Map.HEIGHT - 2))
                    {
                        // Seek up and find the first non-set tile
                        for (int y = Map.HEIGHT - 2; y >= 0; y--)
                        {
                            if (!battlefield.Get(x, y))
                            {
                                // Do a gravity step and make sure it doesn't slip down
                                battlefield.GravityStep();
                                if (!battlefield.Get(x, y + 1))
                                {
                                    SetErrorDescription("Moved down terrain even though there was no room");
                                    return false;
                                }

                                // Destroy the bottom-most tile
                                battlefield.TerrainDestruction(x, Map.HEIGHT - 1, 0.5f);

                                // Do a gravity step and make sure it does slip down
                                battlefield.GravityStep();

                                if (battlefield.Get(x, y + 1))
                                {
                                    SetErrorDescription("Terrain didn't fall");
                                    return false;
                                }

                                // Otherwise this seems to have worked
                                return true;
                            }
                        }


                    }
                }
            }
            SetErrorDescription("Did not find any appropriate terrain to test");
            return false;
        }
        private static bool TestAttackEffect0RecordCurrentGame()
        {
            Requires(TestBoom0Boom);
            Requires(TestGame0Game);

            AttackEffect weaponEffect = new Boom(1, 1, 1);
            Game game = new Game(2, 1);
            weaponEffect.RecordCurrentGame(game);
            return true;
        }
        private static bool TestBullet0Bullet()
        {
            Requires(TestBoom0Boom);
            GenericPlayer player = CreateTestingPlayer();
            Boom explosion = new Boom(1, 1, 1);
            Bullet projectile = new Bullet(25, 25, 45, 30, 0.02f, explosion, player);
            return true;
        }
        private static bool TestBullet0Tick()
        {
            Requires(TestGame0CommenceGame);
            Requires(TestBoom0Boom);
            Requires(TestBullet0Bullet);
            Requires(TestAttackEffect0RecordCurrentGame);
            Game game = InitialiseGame();
            game.CommenceGame();
            GenericPlayer player = game.GetPlayerById(1);
            Boom explosion = new Boom(1, 1, 1);

            Bullet projectile = new Bullet(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.RecordCurrentGame(game);
            projectile.Tick();

            // We can't really test this one without a substantial framework,
            // so we just call it and hope that everything works out

            return true;
        }
        private static bool TestBullet0Paint()
        {
            Requires(TestGame0CommenceGame);
            Requires(TestGame0GetPlayerById);
            Requires(TestBoom0Boom);
            Requires(TestBullet0Bullet);
            Requires(TestAttackEffect0RecordCurrentGame);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the projectile
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            game.CommenceGame();
            GenericPlayer player = game.GetPlayerById(1);
            Boom explosion = new Boom(1, 1, 1);

            Bullet projectile = new Bullet(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.RecordCurrentGame(game);
            projectile.Paint(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestBoom0Boom()
        {
            GenericPlayer player = CreateTestingPlayer();
            Boom explosion = new Boom(1, 1, 1);

            return true;
        }
        private static bool TestBoom0Explode()
        {
            Requires(TestBoom0Boom);
            Requires(TestAttackEffect0RecordCurrentGame);
            Requires(TestGame0GetPlayerById);
            Requires(TestGame0CommenceGame);

            Game game = InitialiseGame();
            game.CommenceGame();
            GenericPlayer player = game.GetPlayerById(1);
            Boom explosion = new Boom(1, 1, 1);
            explosion.RecordCurrentGame(game);
            explosion.Explode(25, 25);

            return true;
        }
        private static bool TestBoom0Tick()
        {
            Requires(TestBoom0Boom);
            Requires(TestAttackEffect0RecordCurrentGame);
            Requires(TestGame0GetPlayerById);
            Requires(TestGame0CommenceGame);
            Requires(TestBoom0Explode);

            Game game = InitialiseGame();
            game.CommenceGame();
            GenericPlayer player = game.GetPlayerById(1);
            Boom explosion = new Boom(1, 1, 1);
            explosion.RecordCurrentGame(game);
            explosion.Explode(25, 25);
            explosion.Tick();

            // Again, we can't really test this one without a full framework

            return true;
        }
        private static bool TestBoom0Paint()
        {
            Requires(TestBoom0Boom);
            Requires(TestAttackEffect0RecordCurrentGame);
            Requires(TestGame0GetPlayerById);
            Requires(TestGame0CommenceGame);
            Requires(TestBoom0Explode);
            Requires(TestBoom0Tick);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the explosion
            GenericPlayer p = CreateTestingPlayer();
            Game game = InitialiseGame();
            game.CommenceGame();
            GenericPlayer player = game.GetPlayerById(1);
            Boom explosion = new Boom(10, 10, 10);
            explosion.RecordCurrentGame(game);
            explosion.Explode(25, 25);
            // Step it for a bit so we can be sure the explosion is visible
            for (int i = 0; i < 10; i++)
            {
                explosion.Tick();
            }
            explosion.Paint(graphics, bitmapSize);

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }

        private static GameForm InitialiseGameForm(out NumericUpDown angleCtrl, out TrackBar powerCtrl, out Button fireCtrl, out Panel controlPanel, out ListBox weaponSelect)
        {
            Requires(TestGame0CommenceGame);

            Game game = InitialiseGame();

            angleCtrl = null;
            powerCtrl = null;
            fireCtrl = null;
            controlPanel = null;
            weaponSelect = null;

            game.CommenceGame();
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Game.CommenceGame() did not create a GameForm and that is the only way GameForm can be tested");
                return null;
            }

            bool foundDisplayPanel = false;
            bool foundControlPanel = false;

            foreach (Control c in gameplayForm.Controls)
            {
                // The only controls should be 2 panels
                if (c is Panel)
                {
                    // Is this the control panel or the display panel?
                    Panel p = c as Panel;

                    // The display panel will have 0 controls.
                    // The control panel will have separate, of which only a few are mandatory
                    int controlsFound = 0;
                    bool foundFire = false;
                    bool foundAngle = false;
                    bool foundAngleLabel = false;
                    bool foundPower = false;
                    bool foundPowerLabel = false;


                    foreach (Control pc in p.Controls)
                    {
                        controlsFound++;

                        // Mandatory controls for the control panel are:
                        // A 'Fire!' button
                        // A NumericUpDown for controlling the angle
                        // A TrackBar for controlling the power
                        // "Power:" and "Angle:" labels

                        if (pc is Label)
                        {
                            Label lbl = pc as Label;
                            if (lbl.Text.ToLower().Contains("angle"))
                            {
                                foundAngleLabel = true;
                            }
                            else
                            if (lbl.Text.ToLower().Contains("power"))
                            {
                                foundPowerLabel = true;
                            }
                        }
                        else
                        if (pc is Button)
                        {
                            Button btn = pc as Button;
                            if (btn.Text.ToLower().Contains("fire"))
                            {
                                foundFire = true;
                                fireCtrl = btn;
                            }
                        }
                        else
                        if (pc is TrackBar)
                        {
                            foundPower = true;
                            powerCtrl = pc as TrackBar;
                        }
                        else
                        if (pc is NumericUpDown)
                        {
                            foundAngle = true;
                            angleCtrl = pc as NumericUpDown;
                        }
                        else
                        if (pc is ListBox)
                        {
                            weaponSelect = pc as ListBox;
                        }
                    }

                    if (controlsFound == 0)
                    {
                        foundDisplayPanel = true;
                    }
                    else
                    {
                        if (!foundFire)
                        {
                            SetErrorDescription("Control panel lacks a \"Fire!\" button OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngle)
                        {
                            SetErrorDescription("Control panel lacks an angle NumericUpDown OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPower)
                        {
                            SetErrorDescription("Control panel lacks a power TrackBar OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngleLabel)
                        {
                            SetErrorDescription("Control panel lacks an \"Angle:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPowerLabel)
                        {
                            SetErrorDescription("Control panel lacks a \"Power:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }

                        foundControlPanel = true;
                        controlPanel = p;
                    }

                }
                else
                {
                    SetErrorDescription(string.Format("Unexpected control ({0}) named \"{1}\" found in GameForm", c.GetType().FullName, c.Name));
                    return null;
                }
            }

            if (!foundDisplayPanel)
            {
                SetErrorDescription("No display panel found");
                return null;
            }
            if (!foundControlPanel)
            {
                SetErrorDescription("No control panel found");
                return null;
            }
            return gameplayForm;
        }

        private static bool TestGameForm0GameForm()
        {
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            return true;
        }
        private static bool TestGameForm0EnableTankControls()
        {
            Requires(TestGameForm0GameForm);
            Game game = InitialiseGame();
            game.CommenceGame();

            // Find the gameplay form
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Game.CommenceGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in GameForm");
                return false;
            }

            // Disable the control panel to check that EnableControlPanel enables it
            controlPanel.Enabled = false;

            gameplayForm.EnableTankControls();

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after GameForm.EnableTankControls()");
                return false;
            }
            return true;

        }
        private static bool TestGameForm0Aim()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            float testAngle = 27;

            gameplayForm.Aim(testAngle);
            if (FloatEquals((float)angle.Value, testAngle)) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set angle to {0} but angle is {1}", testAngle, (float)angle.Value));
                return false;
            }
        }
        private static bool TestGameForm0SetTankPower()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            int testPower = 71;

            gameplayForm.SetTankPower(testPower);
            if (power.Value == testPower) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set power to {0} but power is {1}", testPower, power.Value));
                return false;
            }
        }
        private static bool TestGameForm0SetWeaponIndex()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            gameplayForm.SetWeaponIndex(0);

            // WeaponSelect is optional behaviour, so it's okay if it's not implemented here, as long as the method works.
            return true;
        }
        private static bool TestGameForm0Fire()
        {
            Requires(TestGameForm0GameForm);
            // This is something we can't really test properly without a proper framework, so for now we'll just click
            // the button and make sure it disables the control panel
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            controlPanel.Enabled = true;
            fire.PerformClick();
            if (controlPanel.Enabled)
            {
                SetErrorDescription("Control panel still enabled immediately after clicking fire button");
                return false;
            }

            return true;
        }
        private static void UnitTests()
        {
            DoTest(TestGame0Game);
            DoTest(TestGame0PlayerCount);
            DoTest(TestGame0GetRounds);
            DoTest(TestGame0SetPlayer);
            DoTest(TestGame0GetPlayerById);
            DoTest(TestGame0GetTankColour);
            DoTest(TestGame0CalculatePlayerPositions);
            DoTest(TestGame0Randomise);
            DoTest(TestGame0CommenceGame);
            DoTest(TestGame0GetBattlefield);
            DoTest(TestGame0CurrentPlayerTank);
            DoTest(TestTankModel0CreateTank);
            DoTest(TestTankModel0DisplayTankSprite);
            DoTest(TestTankModel0LineDraw);
            DoTest(TestTankModel0GetArmour);
            DoTest(TestTankModel0ListWeapons);
            DoTest(TestGenericPlayer0PlayerController);
            DoTest(TestGenericPlayer0CreateTank);
            DoTest(TestGenericPlayer0Identifier);
            DoTest(TestGenericPlayer0GetTankColour);
            DoTest(TestGenericPlayer0Winner);
            DoTest(TestGenericPlayer0GetWins);
            DoTest(TestPlayerController0StartRound);
            DoTest(TestPlayerController0BeginTurn);
            DoTest(TestPlayerController0HitPos);
            DoTest(TestBattleTank0BattleTank);
            DoTest(TestBattleTank0GetPlayerById);
            DoTest(TestBattleTank0CreateTank);
            DoTest(TestBattleTank0GetPlayerAngle);
            DoTest(TestBattleTank0Aim);
            DoTest(TestBattleTank0GetPower);
            DoTest(TestBattleTank0SetTankPower);
            DoTest(TestBattleTank0GetCurrentWeapon);
            DoTest(TestBattleTank0SetWeaponIndex);
            DoTest(TestBattleTank0Paint);
            DoTest(TestBattleTank0GetX);
            DoTest(TestBattleTank0YPos);
            DoTest(TestBattleTank0Fire);
            DoTest(TestBattleTank0DamageArmour);
            DoTest(TestBattleTank0IsAlive);
            DoTest(TestBattleTank0GravityStep);
            DoTest(TestMap0Map);
            DoTest(TestMap0Get);
            DoTest(TestMap0CheckTankCollide);
            DoTest(TestMap0TankVerticalPosition);
            DoTest(TestMap0TerrainDestruction);
            DoTest(TestMap0GravityStep);
            DoTest(TestAttackEffect0RecordCurrentGame);
            DoTest(TestBullet0Bullet);
            DoTest(TestBullet0Tick);
            DoTest(TestBullet0Paint);
            DoTest(TestBoom0Boom);
            DoTest(TestBoom0Explode);
            DoTest(TestBoom0Tick);
            DoTest(TestBoom0Paint);
            DoTest(TestGameForm0GameForm);
            DoTest(TestGameForm0EnableTankControls);
            DoTest(TestGameForm0Aim);
            DoTest(TestGameForm0SetTankPower);
            DoTest(TestGameForm0SetWeaponIndex);
            DoTest(TestGameForm0Fire);
        }
        
        #endregion
        
        #region CheckClasses

        private static bool CheckClasses()
        {
            string[] classNames = new string[] { "Program", "ComputerOpponent", "Map", "Boom", "GameForm", "Game", "PlayerController", "Bullet", "GenericPlayer", "BattleTank", "TankModel", "AttackEffect" };
            string[][] classFields = new string[][] {
                new string[] { "Main" }, // Program
                new string[] { }, // ComputerOpponent
                new string[] { "Get","CheckTankCollide","TankVerticalPosition","TerrainDestruction","GravityStep","WIDTH","HEIGHT"}, // Map
                new string[] { "Explode" }, // Boom
                new string[] { "EnableTankControls","Aim","SetTankPower","SetWeaponIndex","Fire","InitBuffer"}, // GameForm
                new string[] { "PlayerCount","GetCurrentRound","GetRounds","SetPlayer","GetPlayerById","GetGameplayTank","GetTankColour","CalculatePlayerPositions","Randomise","CommenceGame","NewRound","GetBattlefield","DisplayTanks","CurrentPlayerTank","AddEffect","ProcessWeaponEffects","RenderEffects","CancelEffect","DetectCollision","DamageArmour","GravityStep","FinaliseTurn","CheckWinner","NextRound","WindSpeed"}, // Game
                new string[] { }, // PlayerController
                new string[] { }, // Bullet
                new string[] { "CreateTank","Identifier","GetTankColour","Winner","GetWins","StartRound","BeginTurn","HitPos"}, // GenericPlayer
                new string[] { "GetPlayerById","CreateTank","GetPlayerAngle","Aim","GetPower","SetTankPower","GetCurrentWeapon","SetWeaponIndex","Paint","GetX","YPos","Fire","DamageArmour","IsAlive","GravityStep"}, // BattleTank
                new string[] { "DisplayTankSprite","LineDraw","CreateBMP","GetArmour","ListWeapons","FireWeapon","CreateTank","WIDTH","HEIGHT","NUM_TANKS"}, // TankModel
                new string[] { "RecordCurrentGame","Tick","Paint"} // AttackEffect
            };

            Assembly assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine("Checking classes for public methods...");
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsPublic)
                {
                    if (type.Namespace != "TankBattle")
                    {
                        Console.WriteLine("Public type {0} is not in the TankBattle namespace.", type.FullName);
                        return false;
                    }
                    else
                    {
                        int typeIdx = -1;
                        for (int i = 0; i < classNames.Length; i++)
                        {
                            if (type.Name == classNames[i])
                            {
                                typeIdx = i;
                                classNames[typeIdx] = null;
                                break;
                            }
                        }
                        foreach (MemberInfo memberInfo in type.GetMembers())
                        {
                            string memberName = memberInfo.Name;
                            bool isInherited = false;
                            foreach (MemberInfo parentMemberInfo in type.BaseType.GetMembers())
                            {
                                if (memberInfo.Name == parentMemberInfo.Name)
                                {
                                    isInherited = true;
                                    break;
                                }
                            }
                            if (!isInherited)
                            {
                                if (typeIdx != -1)
                                {
                                    bool fieldFound = false;
                                    if (memberName[0] != '.')
                                    {
                                        foreach (string allowedFields in classFields[typeIdx])
                                        {
                                            if (memberName == allowedFields)
                                            {
                                                fieldFound = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fieldFound = true;
                                    }
                                    if (!fieldFound)
                                    {
                                        Console.WriteLine("The public field \"{0}\" is not one of the authorised fields for the {1} class.\n", memberName, type.Name);
                                        Console.WriteLine("Remove it or change its access level.");
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    //Console.WriteLine("{0} passed.", type.FullName);
                }
            }
            for (int i = 0; i < classNames.Length; i++)
            {
                if (classNames[i] != null)
                {
                    Console.WriteLine("The class \"{0}\" is missing.", classNames[i]);
                    return false;
                }
            }
            Console.WriteLine("All public methods okay.");
            return true;
        }
        
        #endregion

        public static void Main()
        {
            if (CheckClasses())
            {
                UnitTests();

                int passed = 0;
                int failed = 0;
                foreach (string key in unitTestResults.Keys)
                {
                    if (unitTestResults[key] == "Passed")
                    {
                        passed++;
                    }
                    else
                    {
                        failed++;
                    }
                }

                Console.WriteLine("\n{0}/{1} unit tests passed", passed, passed + failed);
                if (failed == 0)
                {
                    Console.WriteLine("Starting up TankBattle...");
                    Program.Main();
                    return;
                }
            }

            Console.WriteLine("\nPress enter to exit.");
            Console.ReadLine();
        }
    }
}
