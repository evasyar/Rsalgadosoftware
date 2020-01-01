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
            RefreshLogs();
            toolTip1.SetToolTip(button2, "Exit Logs");
            toolTip1.SetToolTip(button3, "Search Logs");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveLogs(tcParent);
        }

        private void RefreshLogs()
        {
            using (LogFactory lf = new LogFactory())
            {
                cachedBindingSource = new BindingSource();
                cachedBindingSource.DataSource = lf.getLog()
                    .Take(100)
                    .OrderByDescending(dt => dt.creationDate);
                dataGridView1.DataSource = cachedBindingSource;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (LogFactory lf = new LogFactory())
            {
                cachedBindingSource = new BindingSource();
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    cachedBindingSource.DataSource = lf.getLog()
                        .Where(rst => Convert.ToString(rst.Id).Contains(textBox1.Text) || 
                        rst.Log.ToLower().Contains(textBox1.Text.ToLower()) || 
                        rst.LogType.ToLower().Contains(textBox1.Text.ToLower()) || 
                        rst.modifiedBy.ToLower().Contains(textBox1.Text.ToLower()) || 
                        rst.creationDate.ToShortDateString().Contains(textBox1.Text))
                        .Take(100)
                        .OrderByDescending(dt => dt.creationDate);
                }
                else
                {
                    cachedBindingSource.DataSource = lf.getLog()
                        .Take(100)
                        .OrderByDescending(dt => dt.creationDate);
                }
                dataGridView1.DataSource = cachedBindingSource;
            }
        }
    }
}
