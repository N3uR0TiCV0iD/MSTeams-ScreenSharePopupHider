using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MSTeams.ScreenSharePopupHider.Helpers
{
    public partial class StartupForm : Form
    {
        const int RIGHT_MARGIN = 5;
        const int TOTAL_STEPS = 20;

        int fadeOutTime;
        int fadeOutStep;

        public StartupForm(string text, Color backColor, int fadeOutDelay, int fadeOutTime)
        {
            InitializeComponent();

            this.textLabel.Text = text;
            this.windowPanel.BackColor = backColor;
            if (GetLuma(backColor) <= 40)
            {
                this.textLabel.ForeColor = Color.White;
            }

            var appIcon = GetApplicationIcon();
            if (appIcon != null)
            {
                this.iconBox.Image = appIcon.ToBitmap();
            }

            this.fadeOutTime = fadeOutTime;
            this.timer.Interval = fadeOutDelay;
            MoveToBottomRightCorner();
        }

        private double GetLuma(Color color)
        {
            return (0.2126 * color.R) + (0.7152 * color.G) + (0.0722 * color.B);
        }

        public Icon? GetApplicationIcon()
        {
            var process = Process.GetCurrentProcess();
            var executablePath = process.MainModule!.FileName!;
            return Icon.ExtractAssociatedIcon(executablePath);
        }

        private void MoveToBottomRightCorner()
        {
            var labelPos = textLabel.Location;
            this.Width = labelPos.X + textLabel.Width + RIGHT_MARGIN;

            var primaryScreen = Screen.PrimaryScreen!;
            var screenBounds = primaryScreen.WorkingArea;
            this.Location = new Point(screenBounds.Width - this.Width, screenBounds.Height - this.Height);
        }

        private void StartFadeOut_Tick(object? sender, EventArgs e)
        {
            timer.Interval = fadeOutTime / TOTAL_STEPS;
            timer.Tick -= StartFadeOut_Tick;
            timer.Tick += FadeOut_Tick;
        }

        private void FadeOut_Tick(object? sender, EventArgs e)
        {
            fadeOutStep++;

            this.Opacity = 1.0 - (fadeOutStep / (float)TOTAL_STEPS);
            if (this.Opacity <= 0)
            {
                timer.Stop();
                this.Close();
            }
        }
    }
}
