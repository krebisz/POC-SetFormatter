
namespace SetFormatterWebClient
{
    partial class TreeForm
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
            // ButtonExport
            // 
            this.ButtonExport = new System.Windows.Forms.Button();
            this.ButtonExport.Location = new System.Drawing.Point(320, 25);
            this.ButtonExport.Name = "ButtonExport";
            this.ButtonExport.Size = new System.Drawing.Size(55, 20);
            this.ButtonExport.TabIndex = 1;
            this.ButtonExport.Text = "Export";
            this.ButtonExport.UseVisualStyleBackColor = true;
            this.ButtonExport.Click += new System.EventHandler(this.ButtonExport_Click);
            // 
            // PanelTrees
            // 
            this.PanelTrees = new System.Windows.Forms.Panel();
            this.PanelTrees.Controls.Add(this.Tree0);
            this.PanelTrees.Controls.Add(this.Tree1);
            this.PanelTrees.Location = new System.Drawing.Point(15, 50);
            this.PanelTrees.Name = "PanelTrees";
            this.PanelTrees.Size = new System.Drawing.Size(360, 415);
            this.PanelTrees.TabIndex = 4;
            this.PanelTrees.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelElements
            // 
            this.LabelElements = new System.Windows.Forms.Label();
            this.LabelElements.AutoSize = true;
            this.LabelElements.Location = new System.Drawing.Point(12, 30);
            this.LabelElements.Name = "LabelElements";
            this.LabelElements.Size = new System.Drawing.Size(50, 13);
            this.LabelElements.TabIndex = 0;
            this.LabelElements.Text = "Elements";
            // 
            // Tree0
            // 
            this.Tree0 = new System.Windows.Forms.TreeView();
            this.Tree0.Location = new System.Drawing.Point(15, 15);
            this.Tree0.Name = "Tree0";
            this.Tree0.Size = new System.Drawing.Size(150, 390);
            this.Tree0.TabIndex = 2;
            // 
            // Tree1
            // 
            this.Tree1 = new System.Windows.Forms.TreeView();
            this.Tree1.Location = new System.Drawing.Point(200, 15);
            this.Tree1.Name = "Tree1";
            this.Tree1.Size = new System.Drawing.Size(150, 390);
            this.Tree1.TabIndex = 3;

            // 
            // TreeForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 480);
            this.Controls.Add(this.PanelTrees);
            this.Controls.Add(this.ButtonExport);
            this.Controls.Add(this.LabelElements);
            this.Name = "TreeForm";
            this.PanelTrees.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion



        public System.Windows.Forms.Button ButtonExport;
        public System.Windows.Forms.Panel PanelTrees;
        public System.Windows.Forms.Label LabelElements;
        public System.Windows.Forms.TreeView Tree0;
        public System.Windows.Forms.TreeView Tree1;
    }
}