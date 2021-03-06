﻿using Rdr2ModManager.Data;
using System.Windows.Forms;
using System;
using System.Linq;
using Rdr2ModManager.Helper;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucTargetMod : UserControl
    {
        public TabControl tcParent { get; set; }
        public BindingSource cachedBindingSource { get; set; }
        public target selectedTarget { get; set; }
        public ucTargetMod(TabControl tcContainer)
        {
            InitializeComponent();

            tcParent = tcContainer;
            selectedTarget = new target();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            TabPageHelper.RemoveModFiles(tcParent);
            TabPageHelper.RemoveMods(tcParent);
            TabPageHelper.RemoveModRoot(tcParent);
        }

        private void ucTargetMod_Load(object sender, System.EventArgs e)
        {
            using (LogFactory log = new LogFactory())
            {
                using (targetCrud crud = new targetCrud())
                {
                    cachedBindingSource = new BindingSource();
                    cachedBindingSource.DataSource = crud.Get().OrderByDescending(elem => elem.creationDate);
                    dataGridView1.DataSource = cachedBindingSource;
                    log.infoLog("Mod target loaded into cache");
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows[0].Selected = true;
                        log.infoLog("1st Mod target selected");
                    }
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, System.EventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count > 0)
            {
                foreach (var item in ((DataGridView)sender).SelectedRows)
                {
                    selectedTarget.Id = Convert.ToString(((DataGridViewRow)item).Cells[0].Value);
                    selectedTarget.root = Convert.ToString(((DataGridViewRow)item).Cells[1].Value);
                    selectedTarget.rootName = Convert.ToString(((DataGridViewRow)item).Cells[2].Value);
                    textBox1.Text = Convert.ToString(((DataGridViewRow)item).Cells[1].Value);
                    textBox2.Text = Convert.ToString(((DataGridViewRow)item).Cells[2].Value);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (LogFactory log = new LogFactory())
            {
                using (targetCrud crud = new targetCrud())
                {
                    cachedBindingSource = new BindingSource();
                    cachedBindingSource.DataSource = crud.Get().OrderByDescending(elem => elem.creationDate); 
                    dataGridView1.DataSource = cachedBindingSource;
                    log.infoLog("Mod target cache refreshed");
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows[0].Selected = true;
                        log.infoLog("1st Mod target selected");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (LogFactory log = new LogFactory())
            {
                using (targetCrud crud = new targetCrud())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(textBox1.Text)) throw new Exception("Root location cannot be empty");
                        if (string.IsNullOrWhiteSpace(textBox2.Text)) throw new Exception("Root name cannot be empty");
                        crud.Post(new target() { 
                            rootName = textBox2.Text, root = textBox1.Text
                        });
                        cachedBindingSource = new BindingSource();
                        cachedBindingSource.DataSource = crud.Post(new target()
                        {
                            rootName = textBox2.Text,
                            root = textBox1.Text
                        }).OrderByDescending(ord => ord.creationDate); 
                        dataGridView1.DataSource = cachedBindingSource;
                        log.infoLog("New mod target posted");
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1.Rows[0].Selected = true;
                            log.infoLog("1st Mod target selected");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.errLog(ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowDialog();
            textBox1.Text = dlg.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //delete
            using (LogFactory log = new LogFactory())
            {
                using (targetCrud crud = new targetCrud())
                {
                    try
                    {
                        crud.Delete(selectedTarget);
                        cachedBindingSource = new BindingSource();
                        cachedBindingSource.DataSource = crud.Get().OrderByDescending(elem => elem.creationDate); 
                        dataGridView1.DataSource = cachedBindingSource;
                        log.infoLog("Mod target deleted");
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1.Rows[0].Selected = true;
                            log.infoLog("1st Mod target selected");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.errLog(ex.Message);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TabPageHelper.AddMods(tcParent, selectedTarget);
        }
    }
}
