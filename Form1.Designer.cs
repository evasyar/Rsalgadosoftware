namespace Rdr2ModManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.modsTab = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // modsTab
            // 
            this.modsTab.Location = new System.Drawing.Point(12, 12);
            this.modsTab.Name = "modsTab";
            this.modsTab.SelectedIndex = 0;
            this.modsTab.Size = new System.Drawing.Size(760, 537);
            this.modsTab.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.modsTab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "RDR2 MODS MANAGER";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl modsTab;
    }
}

