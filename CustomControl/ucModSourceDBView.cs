﻿using Rdr2ModManager.Data;
using Rdr2ModManager.Helper;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucModSourceDBView : UserControl
    {
        public TabControl tcParent { get; set; }
        public BindingSource cachedBindingSource { get; set; }
        public ucModSourceDBView(TabControl tcContainer)
        {
            InitializeComponent();
            tcParent = tcContainer;
            using (modSourceCrud msc = new modSourceCrud())
            {
                cachedBindingSource = new BindingSource();
                cachedBindingSource.DataSource = msc.Get().OrderByDescending(dt => dt.creationDate);
                dataGridView1.DataSource = cachedBindingSource;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveModSourceDBView(tcParent);
        }
    }
}
