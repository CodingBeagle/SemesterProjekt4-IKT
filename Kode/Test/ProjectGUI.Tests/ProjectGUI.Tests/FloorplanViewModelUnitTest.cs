using System.Diagnostics;
using DatabaseAPI;
using DatabaseAPI.Factories;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace ProjectGUI.Tests
{
    [TestFixture]
    public class FloorplanViewModelUnitTests
    {
        private IStoreDatabaseFactory _dbFactory;
        private DatabaseService _db;
        private IBrowseFileService _fileBrowseService;
        private FloorplanViewModel _uut;

        [SetUp]
        public void Setup()
        {
            _dbFactory = Substitute.For<IStoreDatabaseFactory>();

            _db = Substitute.For<DatabaseService>(_dbFactory);

            _fileBrowseService = Substitute.For<IBrowseFileService>();
            _fileBrowseService.OpenFileDialog().Returns(true);
            _fileBrowseService.FileName.Returns("FileName");

            _uut = new FloorplanViewModel(_db, _fileBrowseService);
        }

        [Test]
        public void FloorplanViewModel_Constructor_RefreshFloorplanThumbnail_DownloadfloorplanCalled()
        {
            
        }

        [Test]
        public void FloorplanViewModel_BrowseFloorplanHandler_FileSelected_SelectedFileNameSet()
        {
            _uut.browseFloorplanHandler();

            Assert.That(string.IsNullOrEmpty(_uut.SelectedFileName), Is.False);
        }
    }
}
