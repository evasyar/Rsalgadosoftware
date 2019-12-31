using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rdr2ModManager.Data;
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
            using (modFileCrud mfc = new modFileCrud())
            {
                cachedBindingSource = new BindingSource();
                cachedBindingSource.DataSource = mfc.Get().OrderByDescending(dt => dt.creationDate);
                dataGridView1.DataSource = cachedBindingSource;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveModFileDBView(tcParent);
        }
    }
}
