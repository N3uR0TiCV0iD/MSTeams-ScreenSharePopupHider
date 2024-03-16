using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MSTeams.ScreenSharePopupHider.Helpers
{
    public static class AutoStartupHelper
    {
        public static void CheckAutoStartup(RegistryKey appRegistryKey, string startupName)
        {
            using (var startupRegistryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                var startupPath = startupRegistryKey!.GetValue(startupName);
                if (startupPath == null)
                {
                    var checkedAutoStartup = appRegistryKey.GetValue("CheckedAutoStartup");
                    if (checkedAutoStartup == null || (int)checkedAutoStartup == 0)
                    {
                        if (MessageBox.Show("Looks like this is the first time you run this application!\n\nWould you like to add it to the Windows startup?",
                                            "Add app to startup?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            AddAppToStartup(startupRegistryKey, startupName);
                        }
                        appRegistryKey.SetValue("CheckedAutoStartup", 1, RegistryValueKind.DWord);
                    }
                }
                else if (startupPath.ToString() != Application.ExecutablePath)
                {
                    if (MessageBox.Show("Looks like you've moved the app since it was last added to the Windows startup...\n\nWould you like to add it to the Windows startup again?",
                                        "App has been moved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        AddAppToStartup(startupRegistryKey, startupName);
                    }
                    else
                    {
                        startupRegistryKey.DeleteValue(startupName);
                    }
                }
            }
        }

        private static void AddAppToStartup(RegistryKey startupRegistryKey, string startupName)
        {
            startupRegistryKey.SetValue(startupName, Application.ExecutablePath, RegistryValueKind.String);
        }
    }
}
