using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rdr2ModManager.Helper;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucStartPage : UserControl
    {
        public TabControl tcParent { get; set; }
        public ucStartPage(TabControl tcContainer)
        {
            InitializeComponent();

            tcParent = tcContainer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            TabPageHelper.AddModRoot(tcParent);
        }
    }
}
