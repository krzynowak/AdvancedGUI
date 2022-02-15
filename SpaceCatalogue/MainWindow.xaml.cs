using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Diagnostics;

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

            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.FullName + "\\SampleData\\ExampleUniverse.xml";
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

                    TreeViewItem mainTreeElem = new TreeViewItem() { Header = $"{nodeI.Name} ( {nodeI.Type} )", Tag = $"{index}/{nodeI.Type}" };

                    treeNode.Items.Add(mainTreeElem);
                    index += 1;
                }

                Debug.WriteLine("HI - congratz on getting here without causing an error");
            }
        }

        void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            TreeViewItem selectedTVI = (TreeViewItem)treeNode.SelectedItem;

            Debug.WriteLine((string)selectedTVI.Header);
            Debug.WriteLine(selectedTVI.Tag);

            string tag = (string)selectedTVI.Tag;
            string[] subs = tag.Split('/');

            int index = Int16.Parse(subs[0]);
            string type = subs[1];

            Debug.WriteLine($"index: {index}");
            Debug.WriteLine($"type: {type}");

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
            new_req.reqList.Add(new DBUniverse.Node_Request(index, type));//the first super clusters

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
                Debug.WriteLine($"Name: {nodeI.Name} Type: {nodeI.Type} Has Children: {nodeI.hasChildren}");

                TreeViewItem mainTreeElem = new TreeViewItem() { Header = $"{nodeI.Name} ( {nodeI.Type} )", Tag = $"{childrenIndex}/{nodeI.Type}" };

                selectedTVI.Items.Add(mainTreeElem);
                childrenIndex += 1;
            }

            selectedTVI.ExpandSubtree();
        }
    }
}
