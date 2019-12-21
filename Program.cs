using Rdr2ModManager.Helper;
using System;
using System.Windows.Forms;

namespace Rdr2ModManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (procAdminLaunch adm = new procAdminLaunch())
            {
                adm.AdminRelauncher();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
