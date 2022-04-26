
namespace SetFormatterWebClient
{
    partial class TreeViewForm
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
            // 
            // TreeViewLabel
            // 
            this.TreeViewLabel = new System.Windows.Forms.Label();
            this.TreeViewLabel.AutoSize = true;
            this.TreeViewLabel.Location = new System.Drawing.Point(12, 30);
            this.TreeViewLabel.Name = "TreeViewLabel";
            this.TreeViewLabel.Size = new System.Drawing.Size(50, 13);
            this.TreeViewLabel.TabIndex = 0;
            this.TreeViewLabel.Text = "Elements";
            // 
            // buttonExport
            // 
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonExport.Location = new System.Drawing.Point(320, 25);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(55, 20);
            this.buttonExport.TabIndex = 1;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // treeViewElements1
            // 
            this.treeViewElements1 = new System.Windows.Forms.TreeView();
            this.treeViewElements1.Location = new System.Drawing.Point(15, 15);
            this.treeViewElements1.Name = "treeViewElements1";
            this.treeViewElements1.Size = new System.Drawing.Size(150, 390);
            this.treeViewElements1.TabIndex = 2;
            // 
            // treeViewElements2
            // 
            this.treeViewElements2 = new System.Windows.Forms.TreeView();
            this.treeViewElements2.Location = new System.Drawing.Point(200, 15);
            this.treeViewElements2.Name = "treeViewElements2";
            this.treeViewElements2.Size = new System.Drawing.Size(150, 390);
            this.treeViewElements2.TabIndex = 3;
            // 
            // panelTrees
            // 
            this.panelTrees = new System.Windows.Forms.Panel();
            this.panelTrees.Controls.Add(this.treeViewElements1);
            this.panelTrees.Controls.Add(this.treeViewElements2);
            this.panelTrees.Location = new System.Drawing.Point(15, 50);
            this.panelTrees.Name = "panelTrees";
            this.panelTrees.Size = new System.Drawing.Size(360, 415);
            this.panelTrees.TabIndex = 4;
            this.panelTrees.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 480);
            this.Controls.Add(this.panelTrees);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.TreeViewLabel);
            this.Name = "TreeViewForm";
            this.panelTrees.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewElements1;
        private System.Windows.Forms.Label TreeViewLabel;
        public System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Panel panelTrees;
        private System.Windows.Forms.TreeView treeViewElements2;
    }
}