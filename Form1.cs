using Rdr2ModManager.Helper;
using System.Windows.Forms;

namespace Rdr2ModManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            TabPageHelper.AddStart(modsTab);
        }
    }
}
