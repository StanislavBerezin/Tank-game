namespace TankBattle
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.displayPanel = new System.Windows.Forms.Panel();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.openFire = new System.Windows.Forms.Button();
            this.lblWeapon = new System.Windows.Forms.Label();
            this.lblPowerValue = new System.Windows.Forms.Label();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.lblPower = new System.Windows.Forms.Label();
            this.lblWind = new System.Windows.Forms.Label();
            this.numUpDownAngle = new System.Windows.Forms.NumericUpDown();
            this.lblWindValue = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.scrollWeapon = new System.Windows.Forms.ComboBox();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.Location = new System.Drawing.Point(0, 32);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(800, 600);
            this.displayPanel.TabIndex = 0;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.displayPanel_Paint);
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel.BackColor = System.Drawing.Color.OrangeRed;
            this.controlPanel.Controls.Add(this.trackBar1);
            this.controlPanel.Controls.Add(this.openFire);
            this.controlPanel.Controls.Add(this.lblWeapon);
            this.controlPanel.Controls.Add(this.lblPowerValue);
            this.controlPanel.Controls.Add(this.lblPlayerName);
            this.controlPanel.Controls.Add(this.lblPower);
            this.controlPanel.Controls.Add(this.lblWind);
            this.controlPanel.Controls.Add(this.numUpDownAngle);
            this.controlPanel.Controls.Add(this.lblWindValue);
            this.controlPanel.Controls.Add(this.lblAngle);
            this.controlPanel.Controls.Add(this.scrollWeapon);
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(800, 44);
            this.controlPanel.TabIndex = 1;
            // 
            // trackBar1
            // 
            this.trackBar1.AccessibleName = "";
            this.trackBar1.Location = new System.Drawing.Point(543, 3);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 5;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 21;
            this.trackBar1.Value = 5;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // openFire
            // 
            this.openFire.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFire.Location = new System.Drawing.Point(695, 8);
            this.openFire.Name = "openFire";
            this.openFire.Size = new System.Drawing.Size(72, 23);
            this.openFire.TabIndex = 20;
            this.openFire.Text = "Fire!";
            this.openFire.UseVisualStyleBackColor = true;
            this.openFire.Click += new System.EventHandler(this.openFire_Click);
            // 
            // lblWeapon
            // 
            this.lblWeapon.AutoSize = true;
            this.lblWeapon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeapon.Location = new System.Drawing.Point(154, 12);
            this.lblWeapon.Name = "lblWeapon";
            this.lblWeapon.Size = new System.Drawing.Size(77, 20);
            this.lblWeapon.TabIndex = 14;
            this.lblWeapon.Text = "Weapon :";
            // 
            // lblPowerValue
            // 
            this.lblPowerValue.AutoSize = true;
            this.lblPowerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPowerValue.Location = new System.Drawing.Point(653, 10);
            this.lblPowerValue.Name = "lblPowerValue";
            this.lblPowerValue.Size = new System.Drawing.Size(27, 20);
            this.lblPowerValue.TabIndex = 19;
            this.lblPowerValue.Text = "20";
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerName.Location = new System.Drawing.Point(3, 15);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(94, 16);
            this.lblPlayerName.TabIndex = 11;
            this.lblPlayerName.Text = "PlayerName";
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPower.Location = new System.Drawing.Point(494, 10);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(61, 20);
            this.lblPower.TabIndex = 18;
            this.lblPower.Text = "Power :";
            // 
            // lblWind
            // 
            this.lblWind.AutoSize = true;
            this.lblWind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWind.Location = new System.Drawing.Point(103, 4);
            this.lblWind.Name = "lblWind";
            this.lblWind.Size = new System.Drawing.Size(36, 13);
            this.lblWind.TabIndex = 12;
            this.lblWind.Text = "Wind";
            // 
            // numUpDownAngle
            // 
            this.numUpDownAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownAngle.Location = new System.Drawing.Point(428, 12);
            this.numUpDownAngle.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numUpDownAngle.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numUpDownAngle.Name = "numUpDownAngle";
            this.numUpDownAngle.Size = new System.Drawing.Size(48, 20);
            this.numUpDownAngle.TabIndex = 17;
            // 
            // lblWindValue
            // 
            this.lblWindValue.AutoSize = true;
            this.lblWindValue.Location = new System.Drawing.Point(103, 20);
            this.lblWindValue.Name = "lblWindValue";
            this.lblWindValue.Size = new System.Drawing.Size(27, 13);
            this.lblWindValue.TabIndex = 13;
            this.lblWindValue.Text = "0 W";
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngle.Location = new System.Drawing.Point(364, 12);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(58, 20);
            this.lblAngle.TabIndex = 16;
            this.lblAngle.Text = "Angle :";
            // 
            // scrollWeapon
            // 
            this.scrollWeapon.FormattingEnabled = true;
            this.scrollWeapon.Location = new System.Drawing.Point(237, 12);
            this.scrollWeapon.Name = "scrollWeapon";
            this.scrollWeapon.Size = new System.Drawing.Size(121, 21);
            this.scrollWeapon.TabIndex = 15;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 629);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.displayPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GameForm";
            this.Text = "Form1";
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button openFire;
        private System.Windows.Forms.Label lblWeapon;
        private System.Windows.Forms.Label lblPowerValue;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label lblWind;
        private System.Windows.Forms.NumericUpDown numUpDownAngle;
        private System.Windows.Forms.Label lblWindValue;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.ComboBox scrollWeapon;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Timer gameTimer;
    }
}

