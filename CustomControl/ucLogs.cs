using Rdr2ModManager.Data;
using Rdr2ModManager.Helper;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucLogs : UserControl
    {
        public TabControl tcParent { get; set; }
        public BindingSource cachedBindingSource { get; set; }
        public ucLogs(TabControl tcContainer)
        {
            InitializeComponent();

            tcParent = tcContainer;
            using (LogFactory lf = new LogFactory())
            {
                cachedBindingSource = new BindingSource();
                cachedBindingSource.DataSource = lf.getLog().OrderByDescending(dt => dt.creationDate);
                dataGridView1.DataSource = cachedBindingSource;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveLogs(tcParent);
        }
    }
}
