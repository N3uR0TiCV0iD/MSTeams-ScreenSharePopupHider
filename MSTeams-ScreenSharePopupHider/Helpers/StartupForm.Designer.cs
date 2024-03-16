namespace MSTeams.ScreenSharePopupHider.Helpers
{
    partial class StartupForm
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
            this.textLabel = new System.Windows.Forms.Label();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.windowPanel = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.windowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textLabel
            // 
            this.textLabel.AutoSize = true;
            this.textLabel.Location = new System.Drawing.Point(43, 13);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(38, 15);
            this.textLabel.TabIndex = 0;
            this.textLabel.Text = "label1";
            // 
            // iconBox
            // 
            this.iconBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iconBox.Location = new System.Drawing.Point(5, 5);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(32, 32);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconBox.TabIndex = 1;
            this.iconBox.TabStop = false;
            // 
            // windowPanel
            // 
            this.windowPanel.BackColor = System.Drawing.SystemColors.Control;
            this.windowPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.windowPanel.Controls.Add(this.iconBox);
            this.windowPanel.Controls.Add(this.textLabel);
            this.windowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowPanel.Location = new System.Drawing.Point(0, 0);
            this.windowPanel.Name = "windowPanel";
            this.windowPanel.Size = new System.Drawing.Size(153, 45);
            this.windowPanel.TabIndex = 2;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.StartFadeOut_Tick);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(153, 45);
            this.Controls.Add(this.windowPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.windowPanel.ResumeLayout(false);
            this.windowPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label textLabel;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.Panel windowPanel;
        private System.Windows.Forms.Timer timer;
    }
}
