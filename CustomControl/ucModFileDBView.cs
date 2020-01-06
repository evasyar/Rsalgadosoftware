using System;
using System.Windows.Forms;
using Rdr2ModManager.Helper;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucModFileDBView : UserControl
    {
        public TabControl tcParent { get; set; }
        public BindingSource cachedBindingSource { get; set; }
        public ucModFileDBView(TabControl tcContainer)
        {
            InitializeComponent();
            tcParent = tcContainer;
            GridViewHelper.GridLoader(dataGridView1, "modfile");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveModFileDBView(tcParent);
        }
    }
}
