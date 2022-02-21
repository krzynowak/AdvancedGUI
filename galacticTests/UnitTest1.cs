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
        public void getNodeInfo_Super_Cluster()
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
            Assert.Equal("Galactic test Group", db_data.myList[0].Name);
            Assert.Equal("Galactic_Group", db_data.myList[0].Type);
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

        [Fact]
        public void getNodeInfo_Galactic_Group()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Galactic test Group Description", db_data.Desc);
            Assert.Equal("test galaxt", db_data.myList[0].Name);
            Assert.Equal("Galaxy", db_data.myList[0].Type);
        }

        [Fact]
        public void getNodeInfo_Galaxy()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("test galax descrition", db_data.Desc);
            Assert.Equal("Planetary system test", db_data.myList[0].Name);
            Assert.Equal("Planetary_System", db_data.myList[0].Type);
            Assert.Equal("test star", db_data.myList[1].Name);
            Assert.Equal("Star", db_data.myList[1].Type);
            Assert.Equal("Compact Star test", db_data.myList[2].Name);
            Assert.Equal("Compact_Star", db_data.myList[2].Type);
        }

        [Fact]
        public void getNodeInfo_Planetary_System()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Planetary system test descrition", db_data.Desc);
            Assert.Equal("test planet", db_data.myList[0].Name);
            Assert.Equal("Planet", db_data.myList[0].Type);
            Assert.Equal("test planet2", db_data.myList[1].Name);
            Assert.Equal("Planet", db_data.myList[1].Type);
            Assert.Equal("test dwarf planet", db_data.myList[2].Name);
            Assert.Equal("Dwarf_Planet", db_data.myList[2].Type);
            Assert.Equal("Asteroid test", db_data.myList[3].Name);
            Assert.Equal("Asteroid", db_data.myList[3].Type);
            Assert.Equal("Meteoroid test", db_data.myList[4].Name);
            Assert.Equal("Meteoroid", db_data.myList[4].Type);
            Assert.Equal("Comet test", db_data.myList[5].Name);
            Assert.Equal("Comet", db_data.myList[5].Type);
            Assert.Equal("Circumstellar_Disk test", db_data.myList[6].Name);
            Assert.Equal("Circumstellar_Disk", db_data.myList[6].Type);
        }

        [Fact]
        public void getNodeInfo_Planet()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planet"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("test planet descrition", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Planet2()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));
            req.reqList.Add(new DBUniverse.Node_Request(2, "Planet"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("test planet2 descrition", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Dwarf_Planet()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Dwarf_Planet"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("dwarf planet desc", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Asteroid()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Asteroid"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Asteroid test desc", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Meteoroid()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Meteoroid"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Meteoroid test.", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Comet()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Comet"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Comet test desc.", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Circumstellar_Disk()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Planetary_System"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Circumstellar_Disk"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Circumstellar_Disk test", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Star()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Star"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("test star desc", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

        [Fact]
        public void getNodeInfo_Compact_Star()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            string connectionString = di.Parent.Parent.Parent.FullName + "\\Sample\\UTSample.xml";
            DBUniverse db = new DBUniverse(connectionString);

            Assert.True(db.connect());

            DBUniverse.DB_Request req = new DBUniverse.DB_Request(1);
            req.reqList.Add(new DBUniverse.Node_Request(1, "Super_Cluster"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galactic_Group"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Galaxy"));
            req.reqList.Add(new DBUniverse.Node_Request(1, "Compact_Star"));

            DBUniverse.DB_Data db_data;
            db_data = db.getNodeInfo(req);

            Assert.Equal("Compact Star test", db_data.Desc);
            Assert.Empty(db_data.myList);
        }

    }
}