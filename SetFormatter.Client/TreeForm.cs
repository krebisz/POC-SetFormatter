using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SetFormatter
{
    public partial class TreeForm : Form
    {
        #region Declarations
        IDictionary<string, object> ObjectLookup;


        public static char[] Delimiters = { '{', '}' };
        public static char Separator = ',';

        public List<Element>[] Sets;
        public TreeView[] TreeViews;
        public ContextMenuStrip TreeContextMenu;

        #endregion Declarations


        #region Load

        public TreeForm()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(640, 480);

            ButtonExport = CreateButton("Export", DockStyle.Right);
            LabelElements = CreateLabel("Elements", DockStyle.Left);
            PanelTrees = CreatePanel("Trees", DockStyle.Bottom);

            LoadData();          

            TreeViews = new TreeView[2];

            for (int i = 0; i < TreeViews.Length; i++)
            {
                //*TEMPORARY STYLING WORKAROUND (TreeViews.Length = 2)*//
                DockStyle style = i % 2 == 0 ? DockStyle.Right : DockStyle.Left;

                TreeViews[i] = CreateTreeView(i, style);

                SetNodes(TreeViews[i]);
                PanelTrees.Controls.Add(TreeViews[i]);
            }

            this.Controls.Add(ButtonExport);
            this.Controls.Add(LabelElements);
            this.Controls.Add(PanelTrees);

            this.ResumeLayout(true);
        }

        public Label CreateLabel(string labelId, DockStyle style)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Location = new Point(12, 30);
            label.Name = "Label" + labelId;
            label.Size = new Size(50, 13);
            label.TabIndex = 0;
            label.Text = labelId;

            label.ClientSize = new Size(50, 13);
            label.Tag = labelId;
            label.Dock = style;

            return label;
        }

        public Button CreateButton(string buttonId, DockStyle style)
        {
            Button button = new Button();
            button.Location = new Point(320, 25);
            button.Name = "Button" + buttonId;
            button.Size = new Size(55, 20);
            button.TabIndex = 1;
            button.Text = buttonId;
            button.UseVisualStyleBackColor = true;
            button.Click += new EventHandler(ButtonExport_Click);

            button.ClientSize = new Size(55, 20);
            button.Tag = buttonId;
            button.Dock = style;
            button.IsAccessible = true;

            return button;
        }

        public Panel CreatePanel(string panelId, DockStyle style)
        {
            Panel panel = new Panel();
            panel.Location = new Point(15, 50);
            panel.Name = "Panel" + panelId;
            panel.Size = new Size(400, 450);
            panel.TabIndex = 4;

            panel.ClientSize = new Size(400, 450);
            panel.Dock = style;

            return panel;
        }

        public TreeView CreateTreeView(int treeId, DockStyle style)
        {
            TreeView treeView = new TreeView();
            treeView.Name = "Tree" + treeId.ToString();
            treeView.Tag = treeId;
            treeView.ClientSize = new Size(150, 400);
            treeView.AllowDrop = true;
            treeView.Dock = style;
            treeView.LabelEdit = true;

            treeView.ItemDrag += new ItemDragEventHandler(TreeNode_Drag);
            treeView.DragEnter += new DragEventHandler(Tree_DragEnter);
            treeView.DragOver += new DragEventHandler(Tree_DragOver);
            treeView.DragDrop += new DragEventHandler(TreeNode_Drop);
            treeView.AfterExpand += new TreeViewEventHandler(TreeNode_Expand);
            treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeNode_Click);
            //treeView.NodeMouseClick += (sender, args) => treeView.SelectedNode = treeView.GetNodeAt(args.X, args.Y);

            return treeView;
        }


        public void CreateObjectDictionary()
        {
            ObjectLookup = new Dictionary<string, object>();

            ObjectLookup.Add("Delimiters", Delimiters);
            ObjectLookup.Add("Separator", Separator);
            ObjectLookup.Add("Sets", Sets);
            ObjectLookup.Add("TreeViews", TreeViews);

            ObjectLookup.Add(TreeContextMenu.Name, TreeContextMenu);
            ObjectLookup.Add(ButtonExport.Name, ButtonExport);
            ObjectLookup.Add(LabelElements.Name, LabelElements);
            ObjectLookup.Add(PanelTrees.Name, PanelTrees);
        }

        #endregion


        #region Events

        private void TreeNode_Drag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
            else if (e.Button == MouseButtons.Right)
            {
                DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        //Set target drop effect to effect in ItemDrag event handler
        private void Tree_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        //Select node under mouse pointer
        private void Tree_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            ((TreeView)sender).SelectedNode = ((TreeView)sender).GetNodeAt(targetPoint);
        }

        private void TreeNode_Drop(object sender, DragEventArgs e)
        {
            Point targetPoint = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode destinationNode = ((TreeView)sender).GetNodeAt(targetPoint);
            TreeNode sourceNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            SetElementsByNodes(sourceNode, destinationNode);

            string draggedNodeTree = sourceNode.TreeView.Name;
            string targetNodeTree = ((TreeView)sender).Name;

            //Confirm drop location node not dragged node or descendant
            if (!sourceNode.Equals(destinationNode) && !ContainsNode(sourceNode, destinationNode) && sourceNode != null && destinationNode != null)
            {
                //If moving, remove node from current and add to drop location
                if (e.Effect == DragDropEffects.Move)
                {
                    sourceNode.Remove();
                    destinationNode.Nodes.Add(sourceNode);
                }

                //If copying, clone and add dragged node to drop location
                else if (e.Effect == DragDropEffects.Copy)
                {
                    destinationNode.Nodes.Add((TreeNode)sourceNode.Clone());
                }

                TreeView treeView = (TreeView)Controls.Find(((TreeView)sender).Name, true)[0];

                UpdateElements(treeView, sourceNode.Name, destinationNode.Name, draggedNodeTree == targetNodeTree ? 2 : 4);
                //RefreshTreeViews(treeView, true, targetNode);
            }
        }

        private void TreeNode_Expand(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                //skip if node data already loaded
                if (node.Tag != null)
                {
                    continue;
                }

                node.Tag = new object();
                //node.Nodes.AddRange(GetDataForThisNode(node));
            }
        }

        private void TreeNode_Click(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = ((TreeView)sender).GetNodeAt(e.X, e.Y);
        }


        public void ContextMenuItem_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Set")
            {
                SetNodes((TreeView)((ContextMenuStrip)sender).SourceControl);
            }
            if (e.ClickedItem.Text == "Add")
            {
                AddNode((TreeView)((ContextMenuStrip)sender).SourceControl);
            }
            if (e.ClickedItem.Text == "Delete")
            {
                DeleteNode((TreeView)((ContextMenuStrip)sender).SourceControl);
            }
            if (e.ClickedItem.Text == "Sort")
            {
                SortNodes((TreeView)((ContextMenuStrip)sender).SourceControl);
            }
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            Set setFormatter = new Set();

            for (int i = 0; i < Sets.Length; i++)
            {
                setFormatter.WriteSetToFile(ref Sets[i]);
            }
        }

        #endregion Events


        #region Tree Operations

        public void SetNodes(TreeView treeView, List<Element> set = null)
        {
            if (object.Equals(set, null))
            {
                set = new List<Element>();

                int i = 0;
                while (set.Count == 0 && i < Sets.Length)
                {
                    if (treeView.Name == "Tree" + i.ToString())
                    {
                        set = Sets[i];
                    }

                    i++;
                }
            }

            List<Element> setOrdered = set.OrderBy(o => o.Level).ThenBy(e => e.Id).ToList();

            foreach (Element element in setOrdered)
            {
                TreeNode node = CreateNode(element.Id, treeView);
                Element parent = set.FirstOrDefault(e => e.Id == element.Parent);

                if (parent != null)
                {
                    TreeNode[] nodes = treeView.Nodes.Find(parent.Id.ToString(), true);

                    if (nodes.Length > 0)
                    {
                        treeView.Nodes.Find(parent.Id.ToString(), true)[0].Nodes.Add(node);
                    }
                    else
                    {
                        treeView.Nodes.Add(node);
                    }
                }
                else
                {
                    treeView.Nodes.Add(node);
                }
            }
        }

        public void AddNode(TreeView treeView)
        {
            int id = treeView.GetNodeCount(true);
            TreeNode node = CreateNode(id, treeView);
            treeView.SelectedNode.Nodes.Add(node);
            UpdateElements(treeView, node.Name, treeView.SelectedNode.Name, 1);
            //RefreshTreeViews(treeView, true, treeView.SelectedNode);
        }

        public void DeleteNode(TreeView treeView)
        {
            string id = treeView.SelectedNode.Name;
            treeView.Nodes.Remove(treeView.SelectedNode);
            UpdateElements(treeView, id, id, 3);
            //RefreshTreeViews(treeView, true, treeView.SelectedNode);
        }

        public void SortNodes(TreeView treeView)
        {
            Set setFormatter = new Set();
            List<Element> set = new List<Element>();

            int i = 0;
            while (set.Count == 0 && i < Sets.Length)
            {
                if (treeView.Name == "Tree" + i.ToString())
                {
                    set = setFormatter.OptimizeIds(Sets[i]);
                    Sets[i] = set;
                }

                i++;
            }

            RefreshTreeViews(treeView, true);
        }

        public TreeNode CreateNode(int id, TreeView treeView)
        {
            string idPrefix = "";

            int i = 0;
            while (idPrefix.Length == 0 && i < Sets.Length)
            {
                if (treeView.Name == "Tree" + i.ToString())
                {
                    idPrefix = "T" + id.ToString() + "_";
                }

                i++;
            }

            TreeNode node = new TreeNode
            {
                Name = idPrefix + "Node" + id.ToString(),
                Text = idPrefix + "Node" + id.ToString(),
                Tag = id,
                ContextMenuStrip = CreateContextMenu(idPrefix, id)
            };

            return node;
        }

        public ContextMenuStrip CreateContextMenu(string idPrefix, int id)
        {
            ToolStripMenuItem addLabel = new ToolStripMenuItem();
            addLabel.Text = "Add";
            addLabel.Name = idPrefix + "add" + id.ToString();
            ToolStripMenuItem deleteLabel = new ToolStripMenuItem();
            deleteLabel.Text = "Delete";
            deleteLabel.Name = idPrefix + "delete" + id.ToString();
            ToolStripMenuItem sortLabel = new ToolStripMenuItem();
            sortLabel.Text = "Sort";
            sortLabel.Name = idPrefix + "sort" + id.ToString();

            TreeContextMenu = new ContextMenuStrip();
            TreeContextMenu.Name = idPrefix + "ContextMenu" + id.ToString();
            TreeContextMenu.Items.AddRange(new ToolStripItem[] { addLabel, deleteLabel, sortLabel });
            TreeContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenuItem_Click);

            return TreeContextMenu;
        }

        private bool ContainsNode(TreeNode parent, TreeNode child)
        {
            if (child == null || child.Parent == null)
            {
                return false;
            }
            if (child.Parent.Equals(parent))
            {
                return true;
            }

            //If parent node not null or equal to first node call ContainsNode recursively using the parent of the second node.
            return ContainsNode(parent, child.Parent);
        }

        public void RefreshTreeViews(TreeView treeView, bool refreshAll, TreeNode targetNode = null)
        {
            List<Control> treeViews = new List<Control>();

            if (refreshAll)
            {
                FindControlsOfType(typeof(TreeView), Controls, ref treeViews);
            }
            else
            {
                treeViews.Add(treeView);
            }

            foreach (TreeView T in treeViews)
            {
                T.Nodes.Clear();
                SetNodes(T);

                if (targetNode != null)
                {
                    T.SelectedNode = T.Nodes.Find(targetNode.Name, true)[0];
                    T.SelectedNode.Expand();
                }
            }
        }

        public void FindControlsOfType(Type type, Control.ControlCollection formControls, ref List<Control> controls)
        {
            foreach (Control control in formControls)
            {
                if (control.GetType() == type)
                    controls.Add(control);
                if (control.Controls.Count > 0)
                    FindControlsOfType(type, control.Controls, ref controls);
            }
        }

        #endregion Tree Operations


        #region Data Operations

        public void UpdateElements(TreeView treeView, string elementName, string parentName, int operation)
        {
            int.TryParse(elementName, out int elementId);
            int.TryParse(parentName, out int parentId);

            Set setClass = new Set();
            List<Element> set = new List<Element>();

            int i = 0;
            while (set.Count == 0 && i < Sets.Length)
            {
                if (treeView.Name == "Tree" + i.ToString())
                {
                    set = Sets[i];
                }

                i++;
            }

            switch (operation)
            {
                case 1:
                    {
                        Element parent = set.First(e => e.Id == parentId);
                        Element element = new Element(elementId, elementId.ToString(), parent.Level + 1, parentId);
                        set.Add(element);

                        break;
                    }
                case 2:
                    {
                        Element element = set.First(e => e.Id == elementId);
                        Element parent = set.First(e => e.Id == parentId);
                        element.Parent = parentId;
                        element.Level = parent.Level + 1;

                        break;
                    }
                case 3:
                    {
                        List<Element> setRemove = new List<Element>();
                        List<Element> setNew = new List<Element>();

                        setRemove = setClass.GetChildElements(elementId, setNew, set);

                        foreach (Element e in setRemove)
                        {
                            set.Remove(e);
                        }

                        break;
                    }
                case 4:
                    {
                        List<Element> setNew = new List<Element>();
                        List<Element> setRemove = new List<Element>();

                        setNew = setClass.GetChildElements(elementId, setRemove, Sets[1]);

                        foreach (Element e in setNew)
                        {
                            Sets[0].Add(e);
                            Sets[1].Remove(e);
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            i = 0;
            while (set.Count == 0 && i < Sets.Length)
            {
                if (treeView.Name == "Tree" + i.ToString())
                {
                    Sets[i] = set;
                }

                i++;
            }
        }

        public List<Element> SetElements(TreeView treeView, TreeNode node, List<Element> set, int level)
        {
            level++;

            foreach (TreeNode t in node.Nodes)
            {
                if (treeView.Nodes.Find(t.Tag.ToString(), true).Length > 0)
                {
                    int parent = -1;

                    if (t.Parent != null)
                    {
                        int.TryParse(t.Parent.Tag.ToString(), out parent);
                    }

                    Element element = new Element(Convert.ToInt32(t.Tag), t.Name, level, parent);
                    set.Add(element);

                    SetElements(treeView, t, set, level);
                }
            }

            level--;

            return set;
        }


        //Nodes => Elements
        public void SetElementsByNodes(TreeNode nodeFrom, TreeNode nodeTo)
        {
            try
            {
                TreeView treeFrom = nodeFrom.TreeView;
                TreeView treeTo = nodeTo.TreeView;

                int treeFromTag = Convert.ToInt32(treeFrom.Tag);
                int treeToTag = Convert.ToInt32(treeTo.Tag);

                int nodeFromTag = Convert.ToInt32(nodeFrom.Tag);
                int nodeToTag = Convert.ToInt32(nodeTo.Tag);

                treeFrom.Nodes.Remove(nodeFrom);
                nodeTo.Nodes.Add(nodeFrom);

                TreeViews[0] = treeTo;  //*NEED TO CREATE CHECK AS TO WHICH IS WHICH*//
                TreeViews[1] = treeFrom;

                List<Element> SetNew0 = new List<Element> { Sets[0][0] };
                List<Element> SetNew1 = new List<Element> { Sets[1][0] };

                int set0NodeLevel = 0;
                int set1NodeLevel = 0;

                Sets[0] = SetElements(TreeViews[0], TreeViews[0].Nodes[0], SetNew0, set0NodeLevel);
                Sets[1] = SetElements(TreeViews[1], TreeViews[1].Nodes[0], SetNew1, set1NodeLevel);

                Set setClass = new Set();
                setClass.OptimizeIds(Sets[0]);
                setClass.OptimizeIds(Sets[1]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void GetObjectID()
        {
            Object o = new object();
            
            //var s = "4d6c5246e447ad3bb4495c17";                        
            //var query1 =  ObjectId.Parse(s);
            //var query2 = Query.EQ("_id", ObjectId.Parse(s));
        }



        //Elements => Nodes
        public void SetNodesByElements(List<Element>[] sets)
        {
            List<Control> treeViews = new List<Control>();
            FindControlsOfType(typeof(TreeView), Controls, ref treeViews);

            foreach (TreeView T in treeViews)
            {
                TreeNode selectedNode = T.SelectedNode;

                T.Nodes.Clear();

                int i = 0;
                List<Element> set = new List<Element>();

                while (set.Count == 0 && i < sets.Length)
                {
                    if (T.Name == "Tree" + i.ToString())
                    {
                        set = Sets[i];
                        SetNodes(T, set);
                    }

                    i++;
                }

                if (selectedNode != null)
                {
                    T.SelectedNode = T.Nodes.Find(selectedNode.Name, true).FirstOrDefault();
                    T.SelectedNode.Expand();
                }
            }
        }

        //public void SetNodesByElements(List<Element> set0, List<Element> set1)
        //{
        //    List<Control> treeViews = new List<Control>();
        //    FindControlsOfType(typeof(TreeView), Controls, ref treeViews);

        //    foreach (TreeView T in treeViews)
        //    {
        //        TreeNode selectedNode = T.SelectedNode;

        //        T.Nodes.Clear();

        //        if (T.Name == "Tree0")
        //        {
        //            SetNodes(T, set0);
        //        }
        //        if (T.Name == "Tree1")
        //        {
        //            SetNodes(T, set1);
        //        }

        //        if (selectedNode != null)
        //        {
        //            T.SelectedNode = T.Nodes.Find(selectedNode.Name, true)[0];
        //            T.SelectedNode.Expand();
        //        }
        //    }
        //}

        #endregion Data Operations


        #region IO

        public void LoadData()
        {
            string[] fileNames = IOHelper.GetFileNames(Separator);
            Sets = OpenSets(fileNames);
            GetSets(fileNames, ref Sets);
        }

        public List<Element>[] OpenSets(string[] fileNames)
        {
            if (!object.Equals(fileNames, null))
            {
                List<Element>[] sets = new List<Element>[fileNames.Length];
                return sets;
            }

            return null;
        }

        public void GetSets(string[] fileNames, ref List<Element>[] sets)
        {
            if (!object.Equals(fileNames, null))
            {
                Set setClass = new Set();

                for (int i = 0; i < fileNames.Length; i++)
                {
                    string fileName = fileNames[i];
                    setClass.GetSetFromFile(ref sets[i], fileName, Delimiters, Separator);
                }
            }
        }

        #endregion IO
    }
}



