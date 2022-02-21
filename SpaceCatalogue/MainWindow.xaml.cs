using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Win32;

namespace SpaceCatalogue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Debug.WriteLine("HI - congratz on getting here without causing an error");
                //Debug.WriteLine(openFileDialog.FileName);
                connectionString = openFileDialog.FileName;
            }

            //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);

            //DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            //string connectionString = di.Parent.Parent.FullName + "\\SampleData\\ExampleUniverse.xml";
            //Connect to database
            DBUniverse db = new DBUniverse(connectionString);


            if (!db.connect())
            {
                MessageBox.Show($"{connectionString} - ERROR");
            }
            else
            {
                //obtain info on root elements to initialize basic view
                DBUniverse.DB_Data root;
                root = db.getRootNodeInfo();
                int index = 1;

                foreach (DBUniverse.Node_Info nodeI in root.myList)
                {
                    Debug.WriteLine($"Name: {nodeI.Name} Type: {nodeI.Type} Has Children: {nodeI.hasChildren}");

                    TreeViewItem mainTreeElem = new TreeViewItem() { Header = $"{nodeI.Name} ( {nodeI.Type} )", Tag = $"{index}/{nodeI.Type}/{nodeI.hasChildren}" };

                    treeNode.Items.Add(mainTreeElem);
                    index += 1;
                }

                Debug.WriteLine("HI - congratz on getting here without causing an error");
            }
        }

        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);

            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }

        void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            TreeViewItem selectedTVI = (TreeViewItem)treeNode.SelectedItem;

            List<string> treePath = new List<string>();

            Debug.WriteLine((string)selectedTVI.Header);
            Debug.WriteLine(selectedTVI.Tag);

            string tag = (string)selectedTVI.Tag;
            string[] subs = tag.Split('/');

            int index = Int16.Parse(subs[0]);
            string type = subs[1];
            bool hasChildren = subs[2] == "True";

            Debug.WriteLine($"index: {index}");
            Debug.WriteLine($"type: {type}");
            Debug.WriteLine($"hasChildren: {hasChildren}");

            treePath.Add(tag);

            TreeViewItem currentNode = selectedTVI;
            bool inProgress = true;
            while(inProgress)
            {
                string currentTag = (string)selectedTVI.Tag;
                string[] currentSubs = currentTag.Split('/');
                bool currentHasChildren = currentSubs[2] == "True";

                Debug.WriteLine($"currentTag: {currentTag}");
                Debug.WriteLine($"currentHasChildren: {currentHasChildren}");

                ItemsControl parent = GetSelectedTreeViewItemParent(currentNode);

                TreeViewItem treeitem = parent as TreeViewItem;

                if (treeitem != null)
                {
                    treePath.Add((string)treeitem.Tag);
                    currentNode = treeitem;
                } else
                {
                    inProgress = false;
                }
            }

            treePath.Reverse();

            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.FullName + "\\SampleData\\ExampleUniverse.xml";
            //Connect to database
            Debug.WriteLine("Connect to database");
            DBUniverse db = new DBUniverse(connectionString);

            if (!db.connect())
            {
                MessageBox.Show($"{connectionString} - ERROR");
            }

            Debug.WriteLine("Looking for records");
            DBUniverse.DB_Request new_req = new DBUniverse.DB_Request(1);//Find the elements of:
            foreach (string path in treePath)
            {
                string[] substring = path.Split('/');

                int index2 = Int16.Parse(substring[0]);
                string type2 = substring[1];

                Debug.WriteLine($"index: {index2}");
                Debug.WriteLine($"type: {type2}");

                new_req.reqList.Add(new DBUniverse.Node_Request(index2, type2));//the first super clusters
            }

            //send request
            Debug.WriteLine("Send request");
            DBUniverse.DB_Data selectedNode;
            selectedNode = db.getNodeInfo(new_req);

            Debug.WriteLine($"Description: {selectedNode.Desc}");

            DescriptionBox.Text = selectedNode.Desc;
            NameBox.Text = (string)selectedTVI.Header;
            TypeBox.Text = type;

            int childrenIndex = 1;
            foreach (DBUniverse.Node_Info nodeI in selectedNode.myList)
            {
                Debug.WriteLine($"Name: {nodeI.Name} Type: {nodeI.Type} Has Children: {nodeI.hasChildren}, index: {childrenIndex}");

                TreeViewItem mainTreeElem = new TreeViewItem() { Header = $"{nodeI.Name} ( {nodeI.Type} )", Tag = $"{childrenIndex}/{nodeI.Type}/{nodeI.hasChildren}" };

                selectedTVI.Items.Add(mainTreeElem);
                childrenIndex += 1;
            }

            selectedTVI.ExpandSubtree();
        }
    }
}
