using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class GameForm : Form
    {
        private Color landscapeColour;
        private Random rng = new Random();
        private Image backgroundImage = null;
        private int levelWidth = 160;
        private int levelHeight = 120;
        private Game currentGame;

        private BufferedGraphics backgroundGraphics;
        private BufferedGraphics gameplayGraphics;
        
        
     
        

        public GameForm(Game game)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            

            int rndNumber = rng.Next(0, 3);
            currentGame = game;
           
            string[] imageFilenames = { "Images\\background1.jpg",
                            "Images\\background2.jpg",
                            "Images\\background3.jpg",
                            "Images\\background4.jpg"};

            backgroundImage = Image.FromFile(imageFilenames[rndNumber]);

            Color[] landscapeColours = { Color.FromArgb(255, 0, 0, 0),
                             Color.FromArgb(255, 73, 58, 47),
                             Color.FromArgb(255, 148, 116, 93),
                             Color.FromArgb(255, 133, 119, 109) };
           
            landscapeColour = landscapeColours[rndNumber];

            InitializeComponent();



            backgroundGraphics = InitBuffer();
            gameplayGraphics = InitBuffer();

            DrawBackground();
            DrawGameplay();
            NewTurn();

        }

        // From https://stackoverflow.com/questions/13999781/tearing-in-my-animation-on-winforms-c-sharp
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        public void EnableTankControls()
        {
            controlPanel.Enabled = true;
        }

        public void Aim(float angle)
        {
            numUpDownAngle.Value = (decimal)angle;
        }

        public void SetTankPower(int power)
        {
           trackBar1.Value = power;
        }
        public void SetWeaponIndex(int weapon)
        {
            scrollWeapon.SelectedItem = weapon;
        }

        public void Fire()
        {
            BattleTank player = currentGame.CurrentPlayerTank();
            player.Fire();
            controlPanel.Enabled = false;

        }

        private void DrawBackground()
        {
            Graphics graphics = backgroundGraphics.Graphics;
            Image background = backgroundImage;
            graphics.DrawImage(backgroundImage, new Rectangle(0, 0, displayPanel.Width, displayPanel.Height));

            Map battlefield = currentGame.GetBattlefield();
            Brush brush = new SolidBrush(landscapeColour);

            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.Get(x, y))
                    {
                        int drawX1 = displayPanel.Width * x / levelWidth;
                        int drawY1 = displayPanel.Height * y / levelHeight;
                        int drawX2 = displayPanel.Width * (x + 1) / levelWidth;
                        int drawY2 = displayPanel.Height * (y + 1) / levelHeight;
                        graphics.FillRectangle(brush, drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1);
                    }
                }
            }
        }

        public BufferedGraphics InitBuffer()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            Graphics graphics = displayPanel.CreateGraphics();
            Rectangle dimensions = new Rectangle(0, 0, displayPanel.Width, displayPanel.Height);
            BufferedGraphics bufferedGraphics = context.Allocate(graphics, dimensions);
            return bufferedGraphics;
        }

        private void displayPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = displayPanel.CreateGraphics();
            gameplayGraphics.Render(graphics);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void openFire_Click(object sender, EventArgs e)
        {
            controlPanel.Enabled = false;
        }

        private void DrawGameplay()
        {
            backgroundGraphics.Render(gameplayGraphics.Graphics);
            currentGame.DisplayTanks(gameplayGraphics.Graphics, displayPanel.Size);
            currentGame.RenderEffects(gameplayGraphics.Graphics, displayPanel.Size);
        }
        private void NewTurn()
        {
            BattleTank player = currentGame.CurrentPlayerTank();
            GenericPlayer tankController = player.GetPlayerById();
            this.Text = "Tank Battle - Round " + currentGame.GetCurrentRound() + "of " + currentGame.GetRounds();
            BackColor = tankController.GetTankColour();
            lblPlayerName.Text = tankController.Identifier();
            Aim(player.GetPlayerAngle());
            SetTankPower(player.GetPower());
            if (currentGame.WindSpeed() > 0)
            {
                lblWindValue.Text = currentGame.WindSpeed() + " E";
            }
            else
            {
                lblWindValue.Text = currentGame.WindSpeed() * -1 + " W";
            }
            scrollWeapon.Items.Clear();
            TankModel tank = player.CreateTank();
            String[] lWeaponsAvailable = tank.ListWeapons();
            scrollWeapon.Items.AddRange(lWeaponsAvailable);
            SetWeaponIndex(player.GetCurrentWeapon());
            tankController.BeginTurn(this, currentGame);
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {

            if (currentGame.ProcessWeaponEffects())
            {
                DrawGameplay();
                displayPanel.Invalidate();
            }
            else
            {
                currentGame.GravityStep();
                DrawBackground();
                DrawGameplay();
                displayPanel.Invalidate();
                if (currentGame.GravityStep())
                {
                    return;
                }
                else
                {
                    gameTimer.Enabled = false;
                    if (currentGame.FinaliseTurn())
                    {
                        NewTurn();
                    }
                    Dispose();
                    currentGame.NextRound();
                    return;
                }
            }
        }
    }
}
