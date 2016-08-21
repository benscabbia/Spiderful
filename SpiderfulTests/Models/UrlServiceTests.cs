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
        public void getUrlsTest_simpleTest()
        {            
            List<string> urls1 = UrlService.getPages("http://example.com/").ToList();
            List<string> urls2 = UrlService.getPages("example.com/").ToList();
            List<string> urls3 = UrlService.getPages("www.example.com/").ToList();
            List<string> urls4 = UrlService.getPages("http://www.example.com/").ToList();

            Assert.AreEqual(urls1.Count, 1);
            Assert.AreEqual(urls1.First(), "http://www.iana.org/domains/example");
            Assert.AreEqual(urls2.Count, 1);
            Assert.AreEqual(urls2.First(), "http://www.iana.org/domains/example");
            Assert.AreEqual(urls3.Count, 1);
            Assert.AreEqual(urls3.First(), "http://www.iana.org/domains/example");
            Assert.AreEqual(urls4.Count, 1);
            Assert.AreEqual(urls4.First(), "http://www.iana.org/domains/example");
            //foreach(string u in urls)
            //{
            //    System.Diagnostics.Debug.WriteLine(u);
            //}
        }

        [TestMethod()]
        public void getUrlsTest_brokenUrl()
        {
            var emptyList = Enumerable.Empty<string>();

            IEnumerable<string> urls1 = UrlService.getPages("g.ebay.com/");
            IEnumerable<string> urls2 = UrlService.getPages("ebay");
            IEnumerable<string> urls3 = UrlService.getPages("");

            Assert.AreEqual(urls1, emptyList);
            Assert.AreEqual(urls2, emptyList);
            Assert.AreEqual(urls3, emptyList);
        }

        [TestMethod()]
        public void getUrlsTest_largeSite()
        {
            int ebay = UrlService.getPages("ebay.co.uk/").Count();
            int stack = UrlService.getPages("http://stackoverflow.com").Count();
                       
            if (ebay + stack < 100)
            {
                Assert.Fail("Two sites should have over 100 results, possibly sites are down or something else...");
            }
        }

        //TODO rename tests to getPages
        //TODO tests for getPages containing more than 1 param
    }
}