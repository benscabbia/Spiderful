using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spiderful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderful.Models.Tests
{
    [TestClass()]
    public class UrlServiceTests
    {
        [TestMethod()]
        public void urlFormatterTest_standardInput()
        {
            //=============================================================================
            //test standard input
            string expected1 = "http://google.com";
            string expected2 = "https://google.com";
            string expected3 = "http://www.google.com";
            string expected4 = "http://www.google.com";

            string result1 = UrlService.urlFormatValidator("http://google.com");
            string result2 = UrlService.urlFormatValidator("https://google.com");
            string result3 = UrlService.urlFormatValidator("http://www.google.com");
            string result4 = UrlService.urlFormatValidator("http://www.google.com");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
            Assert.AreEqual(expected3, result3);
            Assert.AreEqual(expected4, result4);

            //=============================================================================
            //test scheme auto formatter
            string expected5 = "http://google.com/";
            string expected6 = "http://next.it/";
            string expected7 = "http://google.co.uk/helloSir/";
            string expected8 = "http://www.google.cc/";

            string result5 = UrlService.urlFormatValidator("google.com");
            string result6 = UrlService.urlFormatValidator("next.it");
            string result7 = UrlService.urlFormatValidator("google.co.uk/helloSir/");
            string result8 = UrlService.urlFormatValidator("www.google.cc");

            Assert.AreEqual(expected5, result5);
            Assert.AreEqual(expected6, result6);
            Assert.AreEqual(expected7, result7);
            Assert.AreEqual(expected8, result8);
        }
        [TestMethod]
        public void urlFormatterTest_secondParam()
        {
            string expected1 = "http://google.com/myPage";
            string expected2 = "https://www.next.it/tech/contact/";
            string expected3 = "https://google.co.uk/helloSir";
            string expected4 = "https://www.google.cc/testa/testb";

            string result1 = UrlService.urlFormatValidator("myPage", "http://google.com");
            string result2 = UrlService.urlFormatValidator("tech/contact/", "https://www.next.it");
            string result3 = UrlService.urlFormatValidator("/helloSir", "https://google.co.uk");
            string result4 = UrlService.urlFormatValidator("/testa/testb", "https://www.google.cc");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
            Assert.AreEqual(expected3, result3);
            Assert.AreEqual(expected4, result4);
        }

        [TestMethod]
        public void urlFormatterTest_secondParamFormatting()
        {
            string expectedEmpty = "";

            string result1 = UrlService.urlFormatValidator("myPage", "google.com");
            string result2 = UrlService.urlFormatValidator("tech/contact/", "www.google.it");
            string result3 = UrlService.urlFormatValidator("/helloSir", "google.co.uk");
            string result4 = UrlService.urlFormatValidator("/testa/testb", "www.google.cc");

            Assert.AreEqual(expectedEmpty, result1);
            Assert.AreEqual(expectedEmpty, result2);
            Assert.AreEqual(expectedEmpty, result3);
            Assert.AreEqual(expectedEmpty, result4);
        }

        [TestMethod()]
        public void urlFormatterTest_brokenUrls()
        {
            string expectedEmpty = "";

            string result1 = UrlService.urlFormatValidator("htp:/");
            string result2 = UrlService.urlFormatValidator("");
            string result3 = UrlService.urlFormatValidator("htp://google.com");
            string result4 = UrlService.urlFormatValidator("google");
            string result5 = UrlService.urlFormatValidator("http://google");
            string result6 = UrlService.urlFormatValidator("https://google");

            Assert.AreEqual(expectedEmpty, result1);
            Assert.AreEqual(expectedEmpty, result2);
            Assert.AreEqual(expectedEmpty, result3);
            Assert.AreEqual(expectedEmpty, result4);
            Assert.AreEqual(expectedEmpty, result5);
            Assert.AreEqual(expectedEmpty, result6);
        }

        [TestMethod()]
        public void getPagesTest_simpleTest()
        {            
            List<string> urls1 = UrlService.getLinks("http://example.com/").ToList();
            List<string> urls1multi = UrlService.getLinks("http://example.com/", false, false).ToList();
            List<string> urls2 = UrlService.getLinks("example.com/", false, false).ToList();
            List<string> urls3 = UrlService.getLinks("www.example.com/", false, false).ToList();
            List<string> urls4 = UrlService.getLinks("http://www.example.com/", false, false).ToList();

            Assert.AreEqual(0, urls1.Count); //0 as host != url so its filtered out
            Assert.AreEqual(1, urls1multi.Count);            
            Assert.AreEqual(1, urls2.Count);
            Assert.AreEqual(urls2.First(), "http://www.iana.org/domains/example");
            Assert.AreEqual(1, urls3.Count);
            Assert.AreEqual(urls3.First(), "http://www.iana.org/domains/example");
            Assert.AreEqual(1, urls4.Count);
            Assert.AreEqual(urls4.First(), "http://www.iana.org/domains/example");
            //foreach(string u in urls)
            //{
            //    System.Diagnostics.Debug.WriteLine(u);
            //}
        }

        [TestMethod()]
        public void getPagesTest_brokenUrl()
        {
            var emptyList = Enumerable.Empty<string>();

            IEnumerable<string> urls1 = UrlService.getLinks("g.ebay.com/");
            IEnumerable<string> urls2 = UrlService.getLinks("ebay");
            IEnumerable<string> urls3 = UrlService.getLinks("");

            Assert.AreEqual(urls1, emptyList);
            Assert.AreEqual(urls2, emptyList);
            Assert.AreEqual(urls3, emptyList);
        }

        [TestMethod()]
        public void getPagesTest_largeSite()
        {
            int ebay = UrlService.getLinks("ebay.co.uk/").Count();
            int stack = UrlService.getLinks("http://stackoverflow.com").Count();
                       
            if (ebay + stack < 100)
            {
                Assert.Fail("Two sites should have over 100 results, possibly sites are down or something else...");
            }
        }

        //TODO tests for getLinks containing more than 1 param

        //TODO multi site links
        [TestMethod()]
        public void getMultiPagesTest_simpleTest()
        {
            List<string> urls1 = UrlService.getLinks("http://example.com/", false, true, 0).ToList();
            Assert.AreEqual(1, urls1.Count);

            List<string> urls2 = UrlService.getLinks("http://example.com/", false, true, 1).ToList();
            Assert.AreEqual(4, urls2.Count);

            List<string> urls3 = UrlService.getLinks("http://example.com/", false, false, 1).ToList();
            Assert.IsTrue(urls3.Count > 30, "Previous tests suggest the count should be 49, maybe site is down OR function broken. Actual Count: {0}", urls3.Count);

            List<string> urls4 = UrlService.getLinks("http://example.com/", false, true, 2).ToList();
            Assert.IsTrue(urls4.Count > 60, "Previous tests suggest the count should be 81, maybe a link is down OR function broken. Actual Count: {0}", urls4.Count);
        }
    }
}