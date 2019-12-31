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
            toolTip1.SetToolTip(button1, "Refresh Logs");
            toolTip1.SetToolTip(button3, "Search Logs");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveLogs(tcParent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshLogs();
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
            using (Prompt pt = new Prompt("Enter your keyword to search under", "Searching logs database"))
            {
                var res = pt.Result;
                using (LogFactory lf = new LogFactory())
                {
                    cachedBindingSource = new BindingSource();
                    cachedBindingSource.DataSource = lf.getLog()
                        .Where(rst => Convert.ToString(rst.Id).ToLower().Contains(res.ToLower()) ||  rst.Log.ToLower().Contains(res.ToLower()) || rst.LogType.ToLower().Contains(res.ToLower()) || rst.modifiedBy.ToLower().Contains(res.ToLower()))
                        .Take(100)
                        .OrderByDescending(dt => dt.creationDate);
                    dataGridView1.DataSource = cachedBindingSource;
                }
            }
        }
    }
}
