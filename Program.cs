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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            using (procAdminLaunch adm = new procAdminLaunch())
            {
                adm.AdminRelauncher();
            }

        }
    }
}
