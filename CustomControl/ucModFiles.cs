using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rdr2ModManager.Data;
using Rdr2ModManager.Helper;

namespace Rdr2ModManager.CustomControl
{
    public partial class ucModFiles : UserControl
    {
        public TabControl tcParent { get; set; }
        public modSource ModSrc { get; set; }
        public target ModTarget { get; set; }
        public BindingSource cachedBindingSource { get; set; }
        public ucModFiles(TabControl tcContainer, modSource ms)
        {
            InitializeComponent();

            tcParent = tcContainer;
            ModSrc = ms;
            //  get the target
            using (targetCrud tc = new targetCrud())
            {
                ModTarget = tc.Get().FirstOrDefault(t => t.Id == ModSrc.TargetId);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPageHelper.RemoveModFiles(tcParent);
        }

        private void FetchModFiles()
        {
            using (LogFactory log = new LogFactory())
            {
                try
                {
                    using (modFileCrud crud = new modFileCrud())
                    {
                        cachedBindingSource = new BindingSource();
                        cachedBindingSource.DataSource = crud.Get()
                            .Where(tid => tid.ModId == ModSrc.Id)
                            .OrderByDescending(dt => dt.creationDate);
                        dataGridView2.DataSource = cachedBindingSource;
                        log.infoLog(string.Format("Mod files listed for mod source {0}", ModSrc.Id));
                        if (dataGridView2.Rows.Count > 0)
                        {
                            dataGridView2.Rows[0].Selected = true;
                            log.infoLog("1st Mod file selected");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.errLog(ex.Message);
                }
            }
        }

        private void ucModFiles_Load(object sender, EventArgs e)
        {
            FetchModFiles();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (LogFactory lf = new LogFactory())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(ModTarget.root)) throw new Exception("MOD file destination should not be empty!");
                    if (string.IsNullOrWhiteSpace(ModSrc.Id)) throw new Exception("MOD ID should not be empty!");
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = (!string.IsNullOrWhiteSpace(ModSrc.Root)) ? ModSrc.Root : @"c:\";
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
                                        DestOneLevel = (FileFolderHelper.IsChildFolder(Path.GetDirectoryName(item), ModSrc.Root)) ? new DirectoryInfo(Path.GetDirectoryName(item)).Name : null,
                                        FileName = Path.GetFileName(item),
                                        ModId = ModSrc.Id,
                                        Source = item
                                    });
                                }
                                FetchModFiles();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lf.errLog(ex.Message);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                using (modFileCrud crud = new modFileCrud())
                {
                    var mark = crud.Get().Where(msc => msc.ModId == ModSrc.Id);
                    foreach (var item in mark)
                    {
                        crud.Del(item);
                    }
                    FetchModFiles();
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
            FetchModFiles();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ModTarget.root)) throw new Exception("MOD file destination should not be empty!");
                if (string.IsNullOrWhiteSpace(ModSrc.Id)) throw new Exception("MOD ID should not be empty!");
                if (string.IsNullOrWhiteSpace(System.IO.Path.Combine(ModSrc.Root, textBox3.Text))) throw new Exception("MOD source should not be empty!");
                if (string.IsNullOrWhiteSpace(textBox3.Text)) throw new Exception("MOD custom file should not be empty!");
                using (modFileCrud crud = new modFileCrud())
                {
                    crud.Post(new modFile()
                    {
                        FileName = textBox3.Text,
                        ModId = ModSrc.Id,
                        Source = System.IO.Path.Combine(ModSrc.Root, textBox3.Text)
                    });
                    FetchModFiles();
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
                    var mods = crud.Get().FindAll(row => row.ModId.ToLower() == ModSrc.Id.ToLower() && !string.IsNullOrWhiteSpace(row.DestOneLevel));
                    foreach (var item in mods)
                    {
                        if (!System.IO.Directory.Exists(System.IO.Path.Combine(ModTarget.root, item.DestOneLevel)))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(ModTarget.root, item.DestOneLevel));
                        }
                        string _dir = System.IO.Path.Combine(ModTarget.root, item.DestOneLevel);
                        System.IO.File.Copy(item.Source, System.IO.Path.Combine(_dir, item.FileName), true);
                        using (LogFactory log = new LogFactory())
                        {
                            log.infoLog(string.Format("File {0} copied to destination: {1}", item.FileName, ModTarget.root));
                        }
                    }
                    mods = crud.Get().FindAll(row => row.ModId.ToLower() == ModSrc.Id.ToLower() && string.IsNullOrWhiteSpace(row.DestOneLevel));
                    foreach (var item in mods)
                    {
                        System.IO.File.Copy(item.Source, System.IO.Path.Combine(ModTarget.root, item.FileName), true);
                        using (LogFactory log = new LogFactory())
                        {
                            log.infoLog(string.Format("File {0} copied to destination: {1}", item.FileName, ModTarget.root));
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
                        var mods = crud.Get().FindAll(row => row.ModId.ToLower() == ModSrc.Id.ToLower() && !string.IsNullOrWhiteSpace(row.DestOneLevel));
                        foreach (var item in mods)
                        {
                            string _dir = System.IO.Path.Combine(ModTarget.root, item.DestOneLevel);
                            try
                            {
                                System.IO.File.Delete(System.IO.Path.Combine(_dir, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, ModTarget.root));
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(item.DestOneLevel)) System.IO.Directory.Delete(_dir, true);
                                log.infoLog(string.Format("Subdirectory {0} deleted from destination: {1}", _dir, ModTarget.root));
                            }
                            catch (Exception ex)
                            {
                                log.errLog(ex.Message);
                            }
                        }
                        mods = crud.Get().FindAll(row => row.ModId.ToLower() == ModSrc.Id.ToLower() && string.IsNullOrWhiteSpace(row.DestOneLevel));
                        foreach (var item in mods)
                        {
                            try
                            {
                                System.IO.File.Delete(System.IO.Path.Combine(ModTarget.root, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, ModTarget.root));
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
                            string _dir = System.IO.Path.Combine(ModTarget.root, item.DestOneLevel);
                            try
                            {
                                System.IO.File.Delete(System.IO.Path.Combine(_dir, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, ModTarget.root));
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
                                System.IO.File.Delete(System.IO.Path.Combine(ModTarget.root, item.FileName));
                                log.infoLog(string.Format("File {0} deleted from destination: {1}", item.FileName, ModTarget.root));
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
