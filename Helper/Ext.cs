using Rdr2ModManager.CustomControl;
using Rdr2ModManager.Data;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace Rdr2ModManager.Helper
{
    public class Prompt : IDisposable
    {
        private Form prompt { get; set; }
        public string Result { get; }

        public Prompt(string text, string caption)
        {
            Result = ShowDialog(text, caption);
        }
        //use a using statement
        private string ShowDialog(string text, string caption)
        {
            prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        public void Dispose()
        {
            //See Marcus comment
            if (prompt != null)
            {
                prompt.Dispose();
            }
        }
    }

    public class procAdminLaunch : IDisposable
    {
        public void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;

                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    System.Windows.Application a = System.Windows.Application.Current;
                    a.Shutdown();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("This program must be run as administrator! \n\n" + ex.ToString());
                }
            }
        }

        private bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~procAdminLaunch()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public static class UserHelper
    {
        public static string GetWinUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
    }

    public static class TabPageHelper
    {
        public static void AddModRoot(TabControl host)
        {
            RemoveModRoot(host);
            host.TabPages.Add("mod root", "mod root");
            host.TabPages["mod root"].Controls.Add(new ucTargetMod(host));
        }

        public static void RemoveModRoot(TabControl host)
        {
            using (LogFactory log = new LogFactory())
            {
                if (host.TabPages.ContainsKey("mod root"))
                {
                    host.TabPages["mod root"].Dispose();
                    log.infoLog("Mod target page removed");
                }
            }
        }

        public static void AddStart(TabControl host)
        {
            RemoveStart(host);
            host.TabPages.Add("start page", "start page");
            host.TabPages["start page"].Controls.Add(new ucStartPage(host));
        }

        public static void RemoveStart(TabControl host)
        {
            using (LogFactory log = new LogFactory())
            {
                if (host.TabPages.ContainsKey("start page"))
                {
                    host.TabPages["start page"].Dispose();
                    log.infoLog("Start page removed");
                }
            }
        }

        public static void AddMods(TabControl host, target _target)
        {
            RemoveMods(host);
            host.TabPages.Add("mods", "mods");
            host.TabPages["mods"].Controls.Add(new ucMods(host, _target));
        }

        public static void RemoveMods(TabControl host)
        {
            using (LogFactory log = new LogFactory())
            {
                if (host.TabPages.ContainsKey("mods"))
                {
                    host.TabPages["mods"].Dispose();
                    log.infoLog("Mods page removed");
                }
            }
        }

        public static void AddModFiles(TabControl host, modSource ms)
        {
            RemoveModFiles(host);
            host.TabPages.Add("mod files", "mod files");
            host.TabPages["mod files"].Controls.Add(new ucModFiles(host, ms));
        }

        public static void RemoveModFiles(TabControl host)
        {
            using (LogFactory log = new LogFactory())
            {
                if (host.TabPages.ContainsKey("mod files"))
                {
                    host.TabPages["mod files"].Dispose();
                    log.infoLog("Mod file page removed");
                }
            }
        }

        public static void AddLogs(TabControl host)
        {
            RemoveLogs(host);
            host.TabPages.Add("logs", "logs");
            host.TabPages["logs"].Controls.Add(new ucLogs(host));
        }

        public static void RemoveLogs(TabControl host)
        {
            using (LogFactory log = new LogFactory())
            {
                if (host.TabPages.ContainsKey("logs"))
                {
                    host.TabPages["logs"].Dispose();
                    log.infoLog("Log page removed");
                }
            }
        }

        public static void AddTargetDBView(TabControl host)
        {
            RemoveTargetDBView(host);
            host.TabPages.Add("targetdbview", "targetdbview");
            host.TabPages["targetdbview"].Controls.Add(new ucTargetDBView(host));
        }

        public static void RemoveTargetDBView(TabControl host)
        {
            using (LogFactory log = new LogFactory())
            {
                if (host.TabPages.ContainsKey("targetdbview"))
                {
                    host.TabPages["targetdbview"].Dispose();
                    log.infoLog("Target DB View page removed");
                }
            }
        }
    }
}
