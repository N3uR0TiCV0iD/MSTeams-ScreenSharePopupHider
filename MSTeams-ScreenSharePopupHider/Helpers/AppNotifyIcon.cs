using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MSTeams.ScreenSharePopupHider.Properties;

namespace MSTeams.ScreenSharePopupHider.Helpers
{
    public class AppNotifyIcon : IDisposable
    {
        public NotifyIcon TrayIcon { get; }

        readonly string appName;
        readonly string copyright;
        readonly string appVersion;

        readonly ContextMenuStrip contextMenu;

        public AppNotifyIcon()
        {
            var appAssembly = Assembly.GetEntryAssembly()!;
            var assemblyInfo = appAssembly.GetName();
            this.appName = assemblyInfo.Name!;
            this.appVersion = assemblyInfo.Version!.ToString(3); //ToString(3) => Only "Major.Minor.Patch"
            this.copyright = GetAssemblyCopyright(appAssembly);

            this.TrayIcon = new NotifyIcon();
            SetupTrayIcon();

            contextMenu = new ContextMenuStrip();
            this.TrayIcon.ContextMenuStrip = contextMenu;
            SetupContextMenu();
        }

        private string GetAssemblyCopyright(Assembly assembly)
        {
            var attributeList = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributeList.Length == 0)
            {
                throw new InvalidOperationException("Copyright information not set.");
            }

            var copyrightAttribute = (AssemblyCopyrightAttribute)attributeList[0];
            return copyrightAttribute.Copyright;
        }

        private void SetupTrayIcon()
        {
            this.TrayIcon.Icon = Resources.Icon;
            this.TrayIcon.Text = $"{appName} v{appVersion}";
            this.TrayIcon.Visible = true;
        }

        private void SetupContextMenu()
        {
            var aboutMenuItem = new ToolStripMenuItem("About");
            var exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Font = new Font(exitMenuItem.Font, FontStyle.Bold);

            contextMenu.Items.Add(aboutMenuItem);
            contextMenu.Items.Add(exitMenuItem);

            aboutMenuItem.Click += aboutMenuItem_Click;
            exitMenuItem.Click += exitMenuItem_Click;
        }

        public void AddMenuItem(ToolStripMenuItem menuItem)
        {
            var items = contextMenu.Items;
            var insertIndex = items.Count - 2;
            if (insertIndex == 0)
            {
                //It is the first item being added.
                //Add a separator between the "custom" and "default" options
                items.Insert(0, new ToolStripSeparator());
            }
            items.Insert(insertIndex, menuItem);
        }

        private void aboutMenuItem_Click(object? sender, EventArgs e)
        {
            var aboutText =
                $"{appName}\n\n" +
                $"Version: {appVersion}\n" +
                $"Author: {copyright}\n";
            MessageBox.Show(aboutText, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitMenuItem_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Dispose()
        {
            contextMenu.Dispose();
            this.TrayIcon.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
