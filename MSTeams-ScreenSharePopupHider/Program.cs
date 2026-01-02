using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using MSTeams.ScreenSharePopupHider.Helpers;

namespace MSTeams.ScreenSharePopupHider
{
    internal static class Program
    {
        static readonly MSTeamsPopupHider msTeamsPopupHider = new MSTeamsPopupHider();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SingleInstanceHelper.CheckAndRun(Startup, SingleInstanceAction.ShowMessage);
        }

        private static void Startup()
        {
            HandleStartup();
            StartApplication();
        }

        private static void HandleStartup()
        {
            using (var appRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\MSTeamsSSPH", true))
            {
                AutoStartupHelper.CheckAutoStartup(appRegistryKey, "MSTeamsSSPH");
                msTeamsPopupHider.ParticipantsHideBehaviour = (HideBehaviour)appRegistryKey.GetValue("ParticipantsHideBehaviour", 0)!;
            }
        }

        private static void StartApplication()
        {
            var appAssembly = Assembly.GetEntryAssembly()!;
            var startupForm = new StartupForm($"{appAssembly.GetName().Name} is now running!", Color.FromArgb(25, 30, 40), 1500, 500);
            startupForm.Show();

            var notifyIcon = new AppNotifyIcon();
            var participantsWindowOptionsMenu = CreateParticipantsWindowOptionsMenu();
            notifyIcon.AddMenuItem(participantsWindowOptionsMenu);

            msTeamsPopupHider.StartMonitoring(notifyIcon.TrayIcon);

            Application.Run();

            notifyIcon.Dispose();
            msTeamsPopupHider.StopMonitoring();
        }

        private static ToolStripMenuItem CreateParticipantsWindowOptionsMenu()
        {
            var optionsMenuItem = new ToolStripMenuItem("Participants Window Behavior");

            var noneItem = new ToolStripMenuItem("None");
            var hideFromTaskbarItem = new ToolStripMenuItem("Hide from taskbar");
            var hideCompletelyItem = new ToolStripMenuItem("Hide completely");

            noneItem.Click += noneItem_Click;
            hideFromTaskbarItem.Click += hideFromTaskbarItem_Click;
            hideCompletelyItem.Click += hideCompletelyItem_Click;

            var dropDownItems = optionsMenuItem.DropDownItems;
            dropDownItems.Add(hideCompletelyItem);
            dropDownItems.Add(hideFromTaskbarItem);
            dropDownItems.Add(noneItem);

            var itemToCheckIndex = 2 - (int)msTeamsPopupHider.ParticipantsHideBehaviour;
            var itemToCheck = (ToolStripMenuItem)dropDownItems[itemToCheckIndex];
            itemToCheck.Checked = true;

            return optionsMenuItem;
        }

        private static void hideCompletelyItem_Click(object? sender, EventArgs e)
        {
            UpdatedCheckedState(sender, HideBehaviour.HideCompletely);
        }

        private static void hideFromTaskbarItem_Click(object? sender, EventArgs e)
        {
            UpdatedCheckedState(sender, HideBehaviour.HideFromTaskbar);
        }

        private static void noneItem_Click(object? sender, EventArgs e)
        {
            UpdatedCheckedState(sender, HideBehaviour.None);
        }

        private static void UpdatedCheckedState(object? sender, HideBehaviour newHideBehaviour)
        {
            using (var appRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\MSTeamsSSPH", true))
            {
                appRegistryKey.SetValue("ParticipantsHideBehaviour", (int)newHideBehaviour, RegistryValueKind.DWord);
            }
            msTeamsPopupHider.ParticipantsHideBehaviour = newHideBehaviour;

            var clickedItem = (ToolStripMenuItem)sender!;
            foreach (ToolStripMenuItem childItem in clickedItem.GetCurrentParent()!.Items)
            {
                childItem.Checked = false;
            }
            clickedItem.Checked = true;
        }
    }
}
