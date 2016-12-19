using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LocatilesWebApp.Controllers;
using LocatilesWebApp.Models;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Unit
{
    [TestFixture]
    public class HomeControllerUnitTest
    {
        private IBLL _bll;
        private Model _model;
        private HomeController _uut;

        [SetUp]
        public void SetUp()
        {
            _bll = Substitute.For<IBLL>();
            _uut = new HomeController(_bll);
        }

        [Test]
        public void SearchItems_SearchItemsCalled_ReturnsIndexView()
        {
            string searchStr = "unittest";
            var result = _uut.SearchItems(searchStr) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void SearchItems_GetPresentationItemGroupsReturnList_ViewBagContainCorrectList()
        {
            string searchStr = "unittest";
            _bll.GetPresentationItemGroups(Arg.Any<string>()).Returns(new List<PresentationItemGroup>() { new PresentationItemGroup("Name", new List<PresentationItem>()) });
            var result = _uut.SearchItems(searchStr) as ViewResult;
            var data = (List<PresentationItemGroup>)(result.ViewData["PresentationItemGroups"]);
            Assert.AreEqual("Name", data[0].Name);
        }
    }
}
