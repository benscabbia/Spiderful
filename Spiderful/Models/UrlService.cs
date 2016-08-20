using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Collections;

namespace Spiderful.Models
{
    public static class UrlService
    {
        /// <summary>
        /// A method used to determine whether a URL is valid. The URL must use a scheme
        /// Valid URL's include: http://test.com, https://www.test.com. http://test.com/page1/page2, etc
        /// </summary>
        /// <param name="url">A string representing the URL</param>
        /// <returns>A boolean determining if the URL is valud</returns>
        private static bool isValid(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static string urlFormatter(string url, string rootUrl = null)
        {
            //url is ok, return
            if ((isValid(url) && url.Contains('.')) || string.IsNullOrEmpty(url)) return url;

            //no root url, try and repair url
            if (string.IsNullOrEmpty(rootUrl))
            {
                //url is not ok - tries to convert google.com -> http://google.com
                UriBuilder link = new UriBuilder(url);
                string generatedUrl = link.Uri.ToString();
                if (isValid(generatedUrl) && generatedUrl.Contains('.')) return generatedUrl;
            }
            else
            {
                //assuming there is a root url, and so could be something lile /contacts, /info, /abc/def
                string fullUrl = "";
                if (isValid(rootUrl))
                {
                    if (url.StartsWith("/") || rootUrl.EndsWith("/")) fullUrl = rootUrl + url;
                    else fullUrl = rootUrl + "/" + url;

                    if (isValid(fullUrl) && fullUrl.Contains('.')) return fullUrl;
                }
            }
            return ""; ;
        }

        public static IEnumerable getUrls(string url, int level = 0)
        {

            HtmlDocument doc = new HtmlWeb().Load(url);

            //var linkTags = doc.DocumentNode.Descendants("link");
            var linkedPages = doc.DocumentNode.Descendants("a")
                                              .Select(a => a.GetAttributeValue("href", null))
                                              .Where(u => !String.IsNullOrEmpty(u));

            List<string> pageUncheckedUrl = new List<string>();


            //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            //{
            //    HtmlAttribute att = link.Attributes["href"];
            //    if (!String.IsNullOrEmpty(att.Value))
            //    {
            //        pageUncheckedUrl.Add(att.Value);
            //    }               
            //}

            return linkedPages;

        }
    }
}
             
 
//        Uri uriResult;
//        bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
//        List<string> hrefTags = new List<string>();
//            if (result)
//            {

//                HtmlWeb hw = new HtmlWeb();
//        HtmlDocument doc = hw.Load(url);
//                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
//                {
//                    HtmlAttribute att = link.Attributes["href"];
//                    if (!String.IsNullOrEmpty(att.Value))
//                    {
//                        hrefTags.Add(att.Value);
//                    }

//}

//                foreach(var a in hrefTags)
//                {
//                    System.Diagnostics.Debug.WriteLine(a.ToString());
//                }
//    }
//}