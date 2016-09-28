using NUnit.Framework;
using SportStore.WebUI.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SportStore.WebUI.Models;
using SportStore.WebUI.HtmlHelpers;

namespace SportStore.WebUI.HtmlHelpers.Tests
{
    [TestFixture()]
    public class PagingHelpersTests
    {
        [Test()]
        public void PageLinksCanGeneratePageLinks()
        {

            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a>"+@"<a class=""btn btn-default btn-primary selected"" href=""Strona2"">2</a>"+@"<a class=""btn btn-default"" href=""Strona3"">3</a>",result.ToString());

        }
    }
}