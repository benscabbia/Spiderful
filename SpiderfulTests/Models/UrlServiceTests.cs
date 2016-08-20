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
        
    }
}