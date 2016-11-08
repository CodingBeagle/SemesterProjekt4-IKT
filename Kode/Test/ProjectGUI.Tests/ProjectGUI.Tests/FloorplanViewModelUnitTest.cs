using System.Diagnostics;
using DatabaseAPI;
using DatabaseAPI.Factories;
using DatabaseAPI.TableFloorplan;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace ProjectGUI.Tests
{
    [TestFixture]
    public class FloorplanViewModelUnitTests
    {
        private IStoreDatabaseFactory _dbFactory;
        private IDatabaseService _db;
        private IBrowseFileService _fileBrowseService;
        private FloorplanViewModel _uut;

        [SetUp]
        public void Setup()
        {
            _dbFactory = Substitute.For<IStoreDatabaseFactory>();

            _db = Substitute.For<IDatabaseService>();

            _fileBrowseService = Substitute.For<IBrowseFileService>();
            _fileBrowseService.OpenFileDialog().Returns(true);
            _fileBrowseService.FileName.Returns("FileName");

            _uut = new FloorplanViewModel(_db, _fileBrowseService);
        }

        [Test]
        public void FloorplanViewModel_Constructor_DownloadfloorplanCalled()
        {
            _db.TableFloorplan.Received(1).DownloadFloorplan(Arg.Any<string>());
        }

        [Test]
        public void FloorplanViewModel_BrowseFloorplanCommand_SelectedFileNameSet()
        {
            _uut.BrowseFloorplanCommand.Execute(null);

            Assert.That(string.IsNullOrEmpty(_uut.SelectedFileName), Is.False);
        }

        [Test]
        public void FloorplanViewModel_UpdateFloorplanCommand_UploadFloorplanCalled()
        {
            _uut.UpdateFloorplanCommand.Execute(null);

            _db.TableFloorplan.Received(1).UploadFloorplan(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>());
        }

        [Test]
        public void FloorplanViewModel_UpdateFloorplanCommand_DeleteAllStoreSectionsCalled()
        {
            _uut.UpdateFloorplanCommand.Execute(null);

            _db.TableStoreSection.Received(1).DeleteAllStoreSections(Arg.Any<long>());
        }

        [Test]
        public void FloorplanViewModel_UpdateFloorplanCommand_DownloadFloorplanCalled()
        {
            _uut.UpdateFloorplanCommand.Execute(null);

            _db.TableFloorplan.Received(2).DownloadFloorplan(Arg.Any<string>());
        }
    }
}
