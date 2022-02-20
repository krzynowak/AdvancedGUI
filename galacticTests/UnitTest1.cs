using Xunit;
using System.IO;
using SpaceCatalogue;
using System;

namespace galacticTests
{
    public class UnitTest1
    {
        [Fact]
        public void loadExistingDatabes()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            System.Console.WriteLine(connectionString);
            //Assert.Equal("C:\\Users\\Michal\\Desktop\\interfejsyproj\\galacticTests\\Sample\\UTSample.xml", connectionString);
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());
        }

        [Fact]
        public void loadNullDatabes()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.FullName + "\\Sample\\UTSampleNotExist.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.False(db.connect());
        }

        [Fact]
        public void readRoot()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Data root;
            root = db.getRootNodeInfo();

            Assert.Equal("Super test cluster", root.myList[0].Name);
            Assert.Equal("Super_Cluster", root.myList[0].Type);
            Assert.True(root.myList[0].hasChildren);
        }

        [Fact]
        public void getNodeInfo()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Super test cluster description.", db_data.Desc);
        }

        [Fact]
        public void getNodeInfo_badRequest()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "No Exist"));

            DBUniverse.DB_Data db_data;

            var ex = Assert.Throws<Exception>(() => db_data = db.getNodeInfo(req));

            Assert.Equal("bad request!!!", ex.Message);
        }


    }
}