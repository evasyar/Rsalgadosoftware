using Rdr2ModManager.Data;
using Rdr2ModManager.Helper;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucTargetDBView : UserControl
    {
        public TabControl tcParent { get; set; }
        public BindingSource cachedBindingSource { get; set; }
        public ucTargetDBView(TabControl tcContainer)
        {
            InitializeComponent();

            tcParent = tcContainer;
            GridViewHelper.GridLoader(dataGridView1, "target");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveTargetDBView(tcParent);
        }        
    }
}
