using Xunit;
using System.IO;
using SpaceCatalogue;

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
        }
    }
}