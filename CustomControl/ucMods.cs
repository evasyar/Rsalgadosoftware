using System;
using System.Linq;
using System.Windows.Forms;
using Rdr2ModManager.Data;
using Rdr2ModManager.Helper;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucMods : UserControl
    {
        public TabControl tcParent { get; set; }
        public target TargetMod { get; set; }
        public BindingSource cachedBindingSource { get; set; }
        public modSource SelectedModSource { get; set; }
        public ucMods(TabControl tcContainer, target _target)
        {
            InitializeComponent();

            tcParent = tcContainer;
            TargetMod = _target;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            TabPageHelper.RemoveModFiles(tcParent);
            TabPageHelper.RemoveMods(tcParent);
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Choose a mod source";
                dlg.ShowDialog();
                textBox2.Text = dlg.SelectedPath;
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            using (LogFactory log = new LogFactory())
            {
                try
                {
                    DateTime relDate = DateTime.Now;
                    using (modSourceCrud crud = new modSourceCrud())
                    {
                        var src = crud.Get().Where(elem => elem.TargetId == TargetMod.Id && elem.Name == textBox1.Text).FirstOrDefault();
                        if (src != null)
                        {
                            throw new Exception(string.Format("Mod name {0} already exists", textBox1.Text));
                        }
                        relDate = (DateTime.TryParse(textBox4.Text, out relDate)) ? relDate : DateTime.Now;
                        var rst = crud.Post(new modSource()
                        {
                            Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox1.Text.ToLower()),
                            Root = textBox2.Text,
                            Version = textBox3.Text,
                            ReleaseDate = relDate,
                            TargetId = TargetMod.Id
                        });
                        dataGridView1.DataSource = new BindingSource() { 
                            DataSource = crud.Get()
                            .Where(tid => tid.TargetId == TargetMod.Id)
                            .OrderByDescending(dt => dt.creationDate) 
                        };
                        log.infoLog("New mod posted");
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1.Rows[0].Selected = true;
                            log.infoLog("1st Mod selected");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count > 0)
            {
                foreach (var item in ((DataGridView)sender).SelectedRows)
                {
                    DateTime tempDate = DateTime.Now;
                    SelectedModSource = new modSource()
                    {
                        Id = Convert.ToString(((DataGridViewRow)item).Cells[0].Value),
                        Root = Convert.ToString(((DataGridViewRow)item).Cells[2].Value),
                        Name = Convert.ToString(((DataGridViewRow)item).Cells[1].Value),
                        TargetId = TargetMod.Id,
                        Version = Convert.ToString(((DataGridViewRow)item).Cells[3].Value),
                        ReleaseDate = DateTime.TryParse(Convert.ToString(((DataGridViewRow)item).Cells[4].Value), out tempDate) ? tempDate : DateTime.Now
                    };
                    textBox1.Text = SelectedModSource.Name;
                    textBox2.Text = SelectedModSource.Root;
                    textBox3.Text = SelectedModSource.Version;
                    textBox4.Text = Convert.ToString(((DataGridViewRow)item).Cells[4].Value);
                }
            }
        }

        private void ucMods_Load(object sender, EventArgs e)
        {
            FetchModSources();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FetchModSources();
        }

        private void FetchModSources()
        {
            using (LogFactory log = new LogFactory())
            {
                try
                {
                    using (modSourceCrud crud = new modSourceCrud())
                    {
                        dataGridView1.DataSource = new BindingSource() { DataSource = crud.Get()
                            .Where(tid => tid.TargetId == TargetMod.Id)
                            .OrderByDescending(dt => dt.creationDate)
                        };
                        log.infoLog(string.Format("Mods listed for mod target {0}", TargetMod.Id));
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1.Rows[0].Selected = true;
                            log.infoLog("1st Mod selected");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TabPageHelper.AddModFiles(tcParent, SelectedModSource);
        }
    }
}
