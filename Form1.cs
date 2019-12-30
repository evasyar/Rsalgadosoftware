using Rdr2ModManager.CustomControl;
using Rdr2ModManager.Data;
using Rdr2ModManager.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Rdr2ModManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            using (targetCrud crud = new targetCrud())
            {
                crud.Get().ForEach(elem =>
                {
                    textBox1.Text = elem.root;
                });
            }

            using (modSourceCrud crud = new modSourceCrud())
            {
                comboBox1.DataSource = crud.Get();
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Id";
                comboBox1.SelectedIndex = -1;
            }

            TabPageHelper.AddModRoot(modsTab);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (targetCrud crud = new targetCrud())
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.ShowDialog();
                target setTarget = new target()
                {
                    root = dlg.SelectedPath
                };
                foreach (var item in crud.Post(setTarget))
                {
                    textBox1.Text = item.root;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Choose a mod root source";
                dlg.ShowDialog();
                textBox2.Text = dlg.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //  data validations
            try
            {
                //  prompt the user for a mod name
                string modName = "";
                string modVersion = "";
                DateTime relDate = DateTime.Now;
                using (Prompt prompt = new Prompt("What is the mod name ?", "Naming a mod"))
                {
                    modName = prompt.Result;
                }
                using (Prompt prompt = new Prompt("What is the mod version ?", "mod version"))
                {
                    modVersion = prompt.Result;
                }
                using (Prompt prompt = new Prompt("What is the version date ?", "mod version date"))
                {
                    relDate = (DateTime.TryParse(prompt.Result, out relDate)) ? relDate : DateTime.Now;
                }
                if (string.IsNullOrWhiteSpace(textBox2.Text)) throw new Exception("MOD root is required");
                if (string.IsNullOrWhiteSpace(modName)) throw new Exception("MOD name is required");

                using (modSourceCrud crud = new modSourceCrud())
                {
                    crud.Post(new modSource()
                    {
                        Name = modName,
                        Root = textBox2.Text,
                        Version = modVersion,
                        ReleaseDate = relDate
                    });

                    comboBox1.DataSource = crud.Get();
                    comboBox1.DisplayMember = "Name";
                    comboBox1.ValueMember = "Id";
                    comboBox1.SelectedIndex = -1;
                    textBox2.Text = "";
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (LogFactory log = new LogFactory())
            {
                var source = new BindingSource();
                source.DataSource = log.getLog();
                dataGridView1.DataSource = source;
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex != -1)
                {
                    string _id = (comboBox1.SelectedItem as modSource).Id;
                    using (modSourceCrud crud = new modSourceCrud())
                    {
                        textBox2.Text = crud.Get().Find(row => row.Id.ToLower().Equals(_id.ToLower())).Root;
                        comboBox1.Text = (comboBox1.SelectedItem as modSource).Name;
                    }
                    using (modFileCrud crud = new modFileCrud())
                    {
                        var source = new BindingSource();
                        source.DataSource = crud.Get().FindAll(row => row.ModId.ToLower() == _id.ToLower());
                        dataGridView2.DataSource = source;
                    }
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex != -1)
                {
                    button13_Click(sender, e);
                    string modId = (comboBox1.SelectedItem as modSource).Id;
                    string modName = (comboBox1.SelectedItem as modSource).Name;
                    using (modSourceCrud crud = new modSourceCrud())
                    {
                        crud.Del(new modSource()
                        {
                            Id = modId,
                            Name = modName
                        });

                        comboBox1.DataSource = crud.Get();
                        comboBox1.DisplayMember = "Name";
                        comboBox1.ValueMember = "Id";
                        comboBox1.SelectedIndex = -1;
                        textBox2.Text = "";
                    }
                    using (modFileCrud crud = new modFileCrud())
                    {
                        var modfiles = crud.Get();
                        var ids = new List<modFile>();
                        modfiles.ForEach(row =>
                        {
                            if (row.ModId.ToLower() == modId.ToLower())
                                ids.Add(row);
                        });
                        foreach (var item in ids)
                        {
                            crud.Del(item);
                        }
                        var source = new BindingSource();
                        source.DataSource = crud.Get().Find(row => row.ModId.ToLower() == modId.ToLower());
                        dataGridView2.DataSource = source;
                    }
                }
                else throw new Exception("You must first select a mod to delete");
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                using (modFileCrud crud = new modFileCrud())
                {
                    var mark = crud.Get().FindAll(row => row.ModId.ToLower() == (comboBox1.SelectedItem as modSource).Id.ToLower());
                    foreach (var item in mark)
                    {
                        crud.Del(item);
                    }

                    var source = new BindingSource();
                    source.DataSource = crud.Get().FindAll(row => row.ModId.ToLower() == (comboBox1.SelectedItem as modSource).Id.ToLower());
                    dataGridView2.DataSource = source;
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                using (modFileCrud crud = new modFileCrud())
                {
                    var source = new BindingSource();
                    source.DataSource = crud.Get().FindAll(row => row.ModId.ToLower() == (comboBox1.SelectedItem as modSource).Id.ToLower());
                    dataGridView2.DataSource = source;
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                using (modFileCrud crud = new modFileCrud())
                {
                    var mods = crud.Get().FindAll(row => row.ModId.ToLower() == (comboBox1.SelectedItem as modSource).Id.ToLower() && !string.IsNullOrWhiteSpace(row.DestOneLevel));
                    foreach (var item in mods)
                    {
                        if (!System.IO.Directory.Exists(System.IO.Path.Combine(textBox1.Text, item.DestOneLevel)))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(textBox1.Text, item.DestOneLevel));
                        }
                        string _dir = System.IO.Path.Combine(textBox1.Text, item.DestOneLevel);
                        System.IO.File.Copy(item.Source, System.IO.Path.Combine(_dir, item.FileName), true);
                        using (LogFactory log = new LogFactory())
                        {
                            log.infoLog(string.Format("File {0} copied to destination: {1}", item.FileName, textBox1.Text));
                        }
                    }
                    mods = crud.Get().FindAll(row => row.ModId.ToLower() == (comboBox1.SelectedItem as modSource).Id.ToLower() && string.IsNullOrWhiteSpace(row.DestOneLevel));
                    foreach (var item in mods)
                    {
                        System.IO.File.Copy(item.Source, System.IO.Path.Combine(textBox1.Text, item.FileName), true);
                        using (LogFactory log = new LogFactory())
                        {
                            log.infoLog(string.Format("File {0} copied to destination: {1}", item.FileName, textBox1.Text));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                using (modFileCrud crud = new modFileCrud())
                {
                    using (LogFactory log = new LogFactory())
                    {
                        var mods = crud.Get().FindAll(row => row.ModId.ToLower() == (comboBox1.SelectedItem as modSource).Id.ToLower() && !string.IsNullOrWhiteSpace(row.DestOneLevel));
                        foreach (var item in mods)
                        {
                            string _dir = System.IO.Path.Combine(textBox1.Text, item.DestOneLevel);
                            try
                            {
                                System.IO.File.Delete(System.IO.Path.Combine(_dir, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, textBox1.Text));
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(item.DestOneLevel)) System.IO.Directory.Delete(_dir, true);
                                log.infoLog(string.Format("Subdirectory {0} deleted from destination: {1}", _dir, textBox1.Text));
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                        }
                        mods = crud.Get().FindAll(row => row.ModId.ToLower() == (comboBox1.SelectedItem as modSource).Id.ToLower() && string.IsNullOrWhiteSpace(row.DestOneLevel));
                        foreach (var item in mods)
                        {
                            try
                            {
                                System.IO.File.Delete(System.IO.Path.Combine(textBox1.Text, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, textBox1.Text));
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                using (LogFactory log = new LogFactory())
                {
                    log.flushLog();
                    var source = new BindingSource();
                    source.DataSource = log.getLog();
                    dataGridView1.DataSource = source;
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string _Destination = textBox1.Text;
                string _ModId = (comboBox1.SelectedItem as modSource).Id;
                if (string.IsNullOrWhiteSpace(_Destination)) throw new Exception("MOD file destination should not be empty!");
                if (string.IsNullOrWhiteSpace(_ModId)) throw new Exception("MOD ID should not be empty!");
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = (!string.IsNullOrWhiteSpace(textBox2.Text)) ? textBox2.Text : "c:\\";
                    openFileDialog.Filter = "All files (*.*)|*.*";
                    openFileDialog.Multiselect = true;

                    if ((openFileDialog.ShowDialog() == DialogResult.OK) && (openFileDialog.FileNames.Length > 0))
                    {
                        using (modFileCrud crud = new modFileCrud())
                        {
                            foreach (var item in openFileDialog.FileNames)
                            {
                                crud.Post(new modFile()
                                {
                                    DestOneLevel = (!string.IsNullOrWhiteSpace(textBox4.Text)) ? textBox4.Text : null,
                                    FileName = Path.GetFileName(item),
                                    ModId = _ModId,
                                    Source = item
                                });
                            }
                            var source = new BindingSource();
                            source.DataSource = crud.Get().FindAll(row => row.ModId.ToLower() == _ModId);
                            dataGridView2.DataSource = source;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string _Destination = textBox1.Text;
                string _ModId = (comboBox1.SelectedItem as modSource).Id;
                if (string.IsNullOrWhiteSpace(_Destination)) throw new Exception("MOD file destination should not be empty!");
                if (string.IsNullOrWhiteSpace(_ModId)) throw new Exception("MOD ID should not be empty!");
                if (string.IsNullOrWhiteSpace(System.IO.Path.Combine(textBox2.Text, textBox3.Text))) throw new Exception("MOD source should not be empty!");
                if (string.IsNullOrWhiteSpace(textBox3.Text)) throw new Exception("MOD custom file should not be empty!");
                using (modFileCrud crud = new modFileCrud())
                {
                    crud.Post(new modFile()
                    {
                        DestOneLevel = (!string.IsNullOrWhiteSpace(textBox4.Text)) ? textBox4.Text : null,
                        FileName = textBox3.Text,
                        ModId = _ModId,
                        Source = System.IO.Path.Combine(textBox2.Text, textBox3.Text)
                    });
                    var source = new BindingSource();
                    source.DataSource = crud.Get().FindAll(row => row.ModId.ToLower() == _ModId);
                    dataGridView2.DataSource = source;
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                using (modFileCrud crud = new modFileCrud())
                {
                    var source = new BindingSource();
                    source.DataSource = crud.Get().AsEnumerable().OrderBy(mod => mod.FileName);
                    dataGridView3.DataSource = source;
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                using (modSourceCrud crud = new modSourceCrud())
                {
                    var source = new BindingSource();
                    source.DataSource = crud.Get().AsEnumerable().OrderBy(mod => mod.Name);
                    dataGridView4.DataSource = source;
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                using (targetCrud crud = new targetCrud())
                {
                    var source = new BindingSource();
                    source.DataSource = crud.Get().AsEnumerable().OrderByDescending(mod => mod.creationDate);
                    dataGridView5.DataSource = source;
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                using (modFileCrud crud = new modFileCrud())
                {
                    using (LogFactory log = new LogFactory())
                    {
                        var mods = crud.Get().FindAll(row => !string.IsNullOrWhiteSpace(row.DestOneLevel));
                        foreach (var item in mods)
                        {
                            string _dir = System.IO.Path.Combine(textBox1.Text, item.DestOneLevel);
                            try
                            {
                                System.IO.File.Delete(System.IO.Path.Combine(_dir, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, textBox1.Text));
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(item.DestOneLevel)) System.IO.Directory.Delete(_dir, true);
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                        }
                        mods = crud.Get().FindAll(row => string.IsNullOrWhiteSpace(row.DestOneLevel));
                        foreach (var item in mods)
                        {
                            try
                            {
                                System.IO.File.Delete(System.IO.Path.Combine(textBox1.Text, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, textBox1.Text));
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }
    }
}
