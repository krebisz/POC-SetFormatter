using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SetFormatterWebClient
{
    public partial class TreeViewForm : Form
    {
        #region Declarations

        public static char[] Delimiters = { '{', '}' };
        public static char Separator = ',';

        public static string fileInPath = ConfigurationManager.AppSettings["fileDirectoryIn"];
        public static string fileOutPath = ConfigurationManager.AppSettings["fileDirectoryOut"];

        private ContextMenuStrip contextMenu;
        //public static List<Element> Elements;
        public List<Element>[] ElementsCollection;
        public TreeView[] TreeViews;

        #endregion Declarations

        #region Load

        public TreeViewForm()
        {
            TreeViews = new TreeView[2];

            TreeViewLabel = CreateLabel("Elements");
            buttonExport = CreateButtonExport("Export");
            panelTrees = CreatePanel("panelTrees", DockStyle.Bottom);

            TreeViews[0] = CreateTreeView(1, DockStyle.Left);
            TreeViews[1] = CreateTreeView(2, DockStyle.Right);


            RetrieveData();
            SetNodes(TreeViews[0]);
            SetNodes(TreeViews[1]);

            this.SuspendLayout();
            this.ClientSize = new Size(640, 480);
            this.Controls.Add(TreeViewLabel);
            this.Controls.Add(buttonExport);

            foreach (TreeView treeView in TreeViews)
            {
                panelTrees.Controls.Add(treeView);
            }

            //panelTrees.Controls.Add(treeViewElements1);
            //panelTrees.Controls.Add(treeViewElements2);
            this.Controls.Add(panelTrees);
            this.ResumeLayout(true);
        }

        public Label CreateLabel(string labelText)
        {
            Label label = new Label();
            label.Text = labelText;
            label.Dock = DockStyle.Left;

            return label;
        }

        public Button CreateButtonExport(string buttonName)
        {
            Button button = new Button();

            button.Name = "button" + buttonName;
            button.Text = buttonName;
            button.ClientSize = new Size(55, 20);
            button.Dock = DockStyle.Right;
            button.IsAccessible = true;
            button.Click += new EventHandler(buttonExport_Click);

            return button;
        }

        public Panel CreatePanel(string panelName, DockStyle style)
        {
            Panel panel = new Panel();

            panel.Name = panelName;
            panel.ClientSize = new Size(400, 450);
            panel.Dock = style;

            return panel;
        }

        public TreeView CreateTreeView(int Id, DockStyle style)
        {
            TreeView treeView = new TreeView();

            treeView = new TreeView();
            treeView.Name = "treeViewElements" + Id.ToString();
            treeView.Tag = Id;
            treeView.ClientSize = new Size(150, 400);
            treeView.AllowDrop = true;
            treeView.Dock = style;
            treeView.LabelEdit = true;
            treeView.ItemDrag += new ItemDragEventHandler(TreeNode_Drag);
            treeView.DragEnter += new DragEventHandler(Tree_DragEnter);
            treeView.DragOver += new DragEventHandler(Tree_DragOver);
            treeView.DragDrop += new DragEventHandler(TreeNode_Drop);
            treeView.Validated += new EventHandler(TreeView_Validation);
            treeView.AfterExpand += AfterExpand;

            treeView.NodeMouseClick += (sender, args) => treeView.SelectedNode = treeView.GetNodeAt(args.X, args.Y);

            return treeView;
        }

        private void TreeView_Validation(object sender, EventArgs e)
        {
            int x = 0;
            x++;
        }


        private void AfterExpand(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                // skip if we have already loaded data for this node
                if (node.Tag != null)
                    continue;
                node.Tag = new object();
                //node.Nodes.AddRange(GetDataForThisNode(node));
            }
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
                // If moving, remove node from current and add to drop location
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

                TreeView treeView = (TreeView)this.Controls.Find(((TreeView)sender).Name, true)[0];



                if (draggedNodeTree == targetNodeTree)
                {
                    UpdateElements(treeView, sourceNode.Name, destinationNode.Name, 2);
                }
                else
                {
                    UpdateElements(treeView, sourceNode.Name, destinationNode.Name, 4);
                }
                //RefreshTreeViews(treeView, true, targetNode);
            }
        }

        public void ContextMenuItem_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Set")
            {
                SetNodes((TreeView)((System.Windows.Forms.ContextMenuStrip)sender).SourceControl);
            }
            if (e.ClickedItem.Text == "Add")
            {
                AddNode((TreeView)((System.Windows.Forms.ContextMenuStrip)sender).SourceControl);
            }
            if (e.ClickedItem.Text == "Delete")
            {
                DeleteNode((TreeView)((System.Windows.Forms.ContextMenuStrip)sender).SourceControl);
            }
            if (e.ClickedItem.Text == "Sort")
            {
                SortNodes((TreeView)((System.Windows.Forms.ContextMenuStrip)sender).SourceControl);
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            foreach (List<Element> elements in ElementsCollection)
            {
                IOHelper.WriteFile(FormatContent(elements));
            }
        }

        #endregion Events

        #region Tree Operations

        public void SetNodes(TreeView treeView, List<Element> elements = null)
        {
            if (object.Equals(elements, null))
            {
                elements = new List<Element>();

                if (treeView.Name == "treeViewElements1")
                {
                    elements = ElementsCollection[0];
                }
                if (treeView.Name == "treeViewElements2")
                {
                    elements = ElementsCollection[1];
                }
            }

            List<Element> _elements = elements.OrderBy(o => o.Level).ThenBy(e => e.Id).ToList();

            foreach (Element element in _elements)
            {
                TreeNode new_node = CreateNode(element.Id, treeView);
                new_node.Tag = element.Id;
                new_node.Name = element.Id.ToString();

                Element parent = elements.FirstOrDefault(e => e.Id == element.Parent);

                if (parent != null)
                {
                    TreeNode[] nodes = treeView.Nodes.Find(parent.Id.ToString(), true);

                    if (nodes.Length > 0)
                    {
                        treeView.Nodes.Find(parent.Id.ToString(), true)[0].Nodes.Add(new_node);
                    }
                    else
                    {
                        treeView.Nodes.Add(new_node);
                    }
                }
                else
                {
                    treeView.Nodes.Add(new_node);
                }
            }
        }

        public void AddNode(TreeView treeView)
        {
            int Id = treeView.GetNodeCount(true);
            TreeNode new_node = CreateNode(Id, treeView);
            new_node.Tag = treeView.Name + "TreeNode" + Id;
            new_node.Name = Id.ToString();

            treeView.SelectedNode.Nodes.Add(new_node);
            UpdateElements(treeView, new_node.Name, treeView.SelectedNode.Name, 1);
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
            List<Element> elements = new List<Element>();

            if (treeView.Name == "treeViewElements1")
            {
                elements = setFormatter.OptimizeIds(ElementsCollection[0]);
                ElementsCollection[0] = elements;
            }
            if (treeView.Name == "treeViewElements2")
            {
                elements = setFormatter.OptimizeIds(ElementsCollection[1]);
                ElementsCollection[1] = elements;
            }

            RefreshTreeViews(treeView, true);
        }

        public TreeNode CreateNode(int Id, TreeView treeView)
        {
            string Id_prefix = "";

            if (treeView.Name == "treeViewElements1")
            {
                Id_prefix = "T1_";
            }
            if (treeView.Name == "treeViewElements2")
            {
                Id_prefix = "T2_";
            }

            TreeNode new_node = new TreeNode(Id.ToString());
            new_node.Text = Id_prefix + String.Format("Node{0}", Id);
            new_node.Name = Id_prefix + "Node" + Id.ToString();

            ToolStripMenuItem addLabel = new ToolStripMenuItem();
            addLabel.Text = "Add";
            addLabel.Name = Id_prefix + "add" + Id.ToString();
            ToolStripMenuItem deleteLabel = new ToolStripMenuItem();
            deleteLabel.Text = "Delete";
            deleteLabel.Name = Id_prefix + "delete" + Id.ToString();
            ToolStripMenuItem sortLabel = new ToolStripMenuItem();
            sortLabel.Text = "Sort";
            sortLabel.Name = Id_prefix + "sort" + Id.ToString();

            contextMenu = new ContextMenuStrip();
            contextMenu.Name = Id_prefix + "contextMenu" + Id.ToString();
            contextMenu.Items.AddRange(new ToolStripMenuItem[] {addLabel, deleteLabel, sortLabel});
            contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenuItem_Clicked);

            new_node.ContextMenuStrip = new ContextMenuStrip();
            new_node.ContextMenuStrip.Name = Id_prefix + "contextMenu" + Id.ToString();
            new_node.ContextMenuStrip = contextMenu;

            return new_node;
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
            TreeNode selectedNode = treeView.SelectedNode;
            List<Control> treeViews = new List<Control>();

            if (refreshAll)
            {
                findControlsOfType(typeof(TreeView), Controls, ref treeViews);
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

        public void findControlsOfType(Type type, Control.ControlCollection formControls, ref List<Control> controls)
        {
            foreach (Control control in formControls)
            {
                if (control.GetType() == type)
                    controls.Add(control);
                if (control.Controls.Count > 0)
                    findControlsOfType(type, control.Controls, ref controls);
            }
        }

        #endregion Tree Operations

        #region Data Operations

        public static List<Element> FormatSets(string Contents)
        {
            Set setFormatter = new Set();
            List<Element> set = setFormatter.CreateSet(Contents, Delimiters, Separator);
            List<Element> formattedSet = set.GroupBy(p => new { p.Id, p.Parent }).Select(g => g.First()).OrderBy(e => (e.Parent, e.Id)).ToList();
            //setFormatter.OrderIds(formattedSet);
            return formattedSet;
        }

        public void UpdateElements(TreeView treeView, string element_name, string parent_name, int operation)
        {
            int.TryParse(element_name, out int element_id);
            int.TryParse(parent_name, out int parent_id);

            List<Element> elements = new List<Element>();

            if (treeView.Name == "treeViewElements1")
            {
                elements = ElementsCollection[0];
            }
            if (treeView.Name == "treeViewElements2")
            {
                elements = ElementsCollection[1];
            }

            switch (operation)
            {
                case 1:
                    {
                        Element parent = elements.First(e => e.Id == parent_id);
                        Element element = new Element(element_id, element_id.ToString(), parent.Level + 1, parent_id);
                        elements.Add(element);

                        break;
                    }
                case 2:
                    {
                        Element element = elements.First(e => e.Id == element_id);
                        Element parent = elements.First(e => e.Id == parent_id);
                        element.Parent = parent_id;
                        element.Level = parent.Level + 1;

                        break;
                    }
                case 3:
                    {
                        List<Element> _elements = new List<Element>();
                        List<Element> elementsRemove = new List<Element>();

                        _elements = GetChildElements(element_id, elementsRemove, elements);

                        foreach (Element e in _elements)
                        {
                            elements.Remove(e);
                        }

                        break;
                    }
                case 4:
                    {
                        List<Element> _elements = new List<Element>();
                        List<Element> elementsRemove = new List<Element>();

                        _elements = GetChildElements(element_id, elementsRemove, ElementsCollection[1]);

                        foreach (Element e in _elements)
                        {
                            ElementsCollection[0].Add(e);
                            ElementsCollection[1].Remove(e);
                        }

                        //Element element = ElementsCollection[1].First(e => e.Id == element_id);

                        //ElementsCollection[0].Add(element);
                        //ElementsCollection[1].Remove(element);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (treeView.Name == "treeViewElements1")
            {
                ElementsCollection[0] = elements;
            }
            if (treeView.Name == "treeViewElements2")
            {
                ElementsCollection[1] = elements;
            }
        }

        public List<Element> GetChildElements(int id, List<Element> _elements, List<Element> elements)
        {
            List<Element> child_elements = new List<Element>();
            child_elements = elements.FindAll(e => e.Parent == id);

            foreach (Element element in child_elements)
            {
                if (elements.Any(e => e.Parent == element.Id))
                {
                    GetChildElements(element.Id, _elements, elements);
                }
                else
                {
                    _elements.Add(element);
                }
            }

            _elements.Add(elements.FirstOrDefault(e => e.Id == id));

            return _elements;
        }




        public List<Element> GetChildNodes(int id, List<Element> _elements, List<Element> elements)
        {
            List<Element> child_elements = new List<Element>();
            child_elements = elements.FindAll(e => e.Parent == id);

            foreach (Element element in child_elements)
            {
                if (elements.Any(e => e.Parent == element.Id))
                {
                    GetChildNodes(element.Id, _elements, elements);
                }
                else
                {
                    _elements.Add(element);
                }
            }

            _elements.Add(elements.FirstOrDefault(e => e.Id == id));

            return _elements;
        }



        public List<Element> SetElements(TreeView treeView, TreeNode treeNode, List<Element> elements, int level)
        {
            List<Element> childElements = new List<Element>();
            //childElements.AddRange(elements);
            level++;

            foreach (TreeNode t in treeNode.Nodes)
            {
                if (treeView.Nodes.Find(t.Tag.ToString(), true).Length > 0)
                {

                    TreeNode activeNode = treeView.Nodes.Find(t.Tag.ToString(), true)[0];

                    int parent = -1;

                    if (t.Parent != null)
                    {
                        int.TryParse(t.Parent.Tag.ToString(), out parent);
                    }

                    Element element = new Element(Convert.ToInt32(t.Tag), t.Name, level, parent);
                    //childElements.Add(element);
                    elements.Add(element);

                    //SetElements(treeView, t, childElements);
                    SetElements(treeView, t, elements, level);
                }
            }

            level--;

            //return childElements;
            return elements;
        }













        //Nodes => Elements
        public void SetElementsByNodes(TreeNode sourceNode, TreeNode destinationNode)
        {
            try
            {
                TreeView sourceTree = sourceNode.TreeView;
                TreeView destinationTree = destinationNode.TreeView;

                int sourceTreeTag = Convert.ToInt32(sourceTree.Tag);
                int sourceNodeTag = Convert.ToInt32(sourceNode.Tag);

                int destinationTreeTag = Convert.ToInt32(destinationTree.Tag);
                int destinationNodeTag = Convert.ToInt32(destinationNode.Tag);

                List<TreeNode> sourceSubTree = new List<TreeNode>();
                sourceTree.Nodes.Remove(sourceNode);
                destinationNode.Nodes.Add(sourceNode);


                TreeViews[0] = destinationTree;
                TreeViews[1] = sourceTree;

                List<Element> NewElementsCollection1 = new List<Element>();
                NewElementsCollection1.Add(ElementsCollection[0][0]);
                List<Element> NewElementsCollection2 = new List<Element>();
                NewElementsCollection2.Add(ElementsCollection[1][0]);

                int collection0NodeLevel = 0;
                int collection1NodeLevel = 0;

                ElementsCollection[0] = SetElements(TreeViews[0], TreeViews[0].Nodes[0], NewElementsCollection1, collection0NodeLevel);
                ElementsCollection[1] = SetElements(TreeViews[1], TreeViews[1].Nodes[0], NewElementsCollection2, collection1NodeLevel);

                Set setFormatter = new Set();
                List<Element> _elements = setFormatter.OptimizeIds(ElementsCollection[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Elements => Nodes
        public void SetNodesByElements(List<Element> elementSet0, List<Element> elementSet1)
        {
            List<Control> treeViews = new List<Control>();


            findControlsOfType(typeof(TreeView), Controls, ref treeViews);


            foreach (TreeView T in treeViews)
            {
                TreeNode selectedNode = T.SelectedNode;

                T.Nodes.Clear();

                if (T.Name == "treeViewElements1")
                {
                    SetNodes(T, elementSet0);
                }
                if (T.Name == "treeViewElements2")
                {
                    SetNodes(T, elementSet1);
                }

                if (selectedNode != null)
                {
                    T.SelectedNode = T.Nodes.Find(selectedNode.Name, true)[0];
                    T.SelectedNode.Expand();
                }
            }
        }

















































        public static string FormatContent(List<Element> elements)
        {
            string formattedContent = string.Empty;

            foreach (Element e in elements)
            {
                formattedContent = formattedContent + "Id:" + e.Id + " Parent:" + e.Parent + " Level:" + e.Level + " |NAME: " + e.Name + "\r\n";
            }

            return formattedContent;
        }

        #endregion Data Operations

        #region IO

        private void RetrieveData()
        {
            string[] Filenames = RetrieveFiles();

            if (!object.Equals(Filenames, null))
            {
                TransformFiles(Filenames, false);
            }
        }

        private string[] RetrieveFiles()
        {
            string[] excludeFilePath = (ConfigurationManager.AppSettings["fileDirectoryExclude"]).Split(new char[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < excludeFilePath.Length; i++)
            {
                excludeFilePath[i] = excludeFilePath[i].Trim();
            }

            string[] Filenames = IOHelper.GetFileNames(fileInPath, excludeFilePath, SearchOption.AllDirectories).ToArray();

            return Filenames;
        }

        private void TransformFiles(string[] Filenames, bool DeleteFile)
        {
            ElementsCollection = new List<Element>[Filenames.Length];

            for (int i = 0; i < Filenames.Length; i++)
            {
                string fileName = Filenames[i];

                try
                {
                    FormatFile(fileName, ref ElementsCollection[i]);

                    if (DeleteFile)
                    {
                        File.Delete(fileName);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static string FormatFile(string fileName, ref List<Element> elements)
        {
            string fileNameOut;

            using (Stream fileStream = IOHelper.GetFileStream(fileName))
            {
                StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
                string fileContent = streamReader.ReadToEnd();

                elements = FormatSets(fileContent);


                string formattedContent = FormatContent(elements);

                fileNameOut = IOHelper.WriteFile(formattedContent);
            }

            return fileNameOut;
        }

        #endregion IO
    }
}



