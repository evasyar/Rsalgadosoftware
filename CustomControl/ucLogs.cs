using Rdr2ModManager.Helper;
using System;
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
            GridViewHelper.RefreshLogs(dataGridView1);
            toolTip1.SetToolTip(button2, "Exit Logs");
            toolTip1.SetToolTip(textBox1, "Enter a search keyword");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveLogs(tcParent);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateDatagridviewFromKeyword((sender as TextBox).Text);
        }

        private void updateDatagridviewFromKeyword(string keyword)
        {
            GridViewHelper.RefreshLogsFromKeyword(dataGridView1, keyword);
        }
    }
}
