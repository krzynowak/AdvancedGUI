using System;
using System.Collections.Generic;
using System.Xml;

namespace SpaceCatalogue
{
    public class DBUniverse
    {
        public struct Node_Info
        {
            public string Name;
            public string Type;
            public bool hasChildren;

            public Node_Info(string name, string typ, bool children)
            {
                Name = name;
                Type = typ;
                hasChildren = children;
            }
        }

        public struct Node_Request
        {
            public int ChildIdx;
            public string Name;

            public Node_Request(int idx, string nme)
            {
                ChildIdx = idx;
                Name = nme;
            }
        }

        public struct DB_Request
        {
            public List<Node_Request> reqList;
            private int reqId;

            public DB_Request(int id)
            {
                reqId = id;
                reqList = new List<Node_Request>();
            }
        }

        public struct DB_Data
        {
            public string Desc;
            public List<Node_Info> myList;// = new List<KeyValuePair>string, string>>();
        }

        private string url;
        private XmlDocument doc;

        public DBUniverse(string url)
        {
            this.url = url;
            this.doc = new XmlDocument();
        }

        public bool connect()
        {
            bool status = false;
            try
            {
                doc.Load(this.url);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            return status;
        }

        public DB_Data getNodeInfo(DB_Request request)
        {
            DB_Data DataInstance = new DB_Data();
            List<Node_Info> tempList = new List<Node_Info>();


            int cnt = 0;
            XmlNode node = this.doc.DocumentElement;


            foreach (Node_Request req in request.reqList)
            {
                node = node[req.Name];
                cnt = req.ChildIdx;
                while (--cnt > 0)
                {
                    if (node.NextSibling == null) { break; }
                    node = node.NextSibling;
                }
            }

            if (node == null) throw new Exception("bad request!!!");

            node = node["Description"];

            DataInstance.Desc = node.InnerText;

            while (node != node.ParentNode.LastChild)
            {
                node = node.NextSibling;
                tempList.Add(new Node_Info(node["name"].InnerText, node.LocalName, node.ChildNodes.Count > 2));
            }

            DataInstance.myList = tempList;

            return DataInstance;
        }


        public DB_Data getRootNodeInfo()
        {
            DB_Data DataInstance = new DB_Data();
            List<Node_Info> tempList = new List<Node_Info>();

            DataInstance.Desc = "Know part of the Multiverse";

            foreach (XmlNode node in this.doc.DocumentElement)
            {
                tempList.Add(new Node_Info(node["name"].InnerText, node.LocalName, true));
            }

            DataInstance.myList = tempList;

            return DataInstance;
        }
    }
}
