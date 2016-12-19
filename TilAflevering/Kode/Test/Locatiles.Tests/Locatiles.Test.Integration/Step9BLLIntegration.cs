using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.Factories;
using LocatilesWebApp.Models;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Integration
{
    [TestFixture]
    public class Step9BLLIntegration
    {
        private IBLL _BLL;
        private ISearcher _searcher;
        [SetUp]
        public void SetUp()
        {
            _searcher = Substitute.For<ISearcher>();
            _BLL = new BLL(_searcher);
        }

        [Test]
        public void GetFloorplan_CalledWithPath_FloorplanIsDownloadedToPath()
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\";
            string file = filepath + @"floorplan.jpg"; //Hardcoded in API
            
            //Remove file
            if (File.Exists(file))
                File.Delete(file);

            _BLL.GetFloorPlan(filepath);

            Assert.That(File.Exists(file));

            //Remove file
            if (File.Exists(file))
                File.Delete(file);
        }
    }
}
