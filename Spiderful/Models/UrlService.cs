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

        /// <summary>
        /// A method which checks a url is well formatted and attempts to repair if not 
        /// It also provides a secondary function of constructing a partial url and root (i.e. myPage + http://google.com)
        /// </summary>
        /// <param name="url">The url to be checked</param>
        /// <param name="rootUrl">An optional url specifying the method will format the URL</param>
        /// <returns>A formatted URL if successful, an empty string if unsuccessful</returns>
        public static string urlFormatValidator(string url, string rootUrl = null)
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

        public static IEnumerable<string> getUrls(string url, int level = 0)
        {
            //make sure url is ok
            string formattedUrl = urlFormatValidator(url);
            
            //if formattedUrl is empty, url is broken
            if (string.IsNullOrEmpty(formattedUrl)) return Enumerable.Empty<string>();

            try
            {
                HtmlDocument doc = new HtmlWeb().Load(formattedUrl);
                //var linkTags = doc.DocumentNode.Descendants("link");
                var linkedPages = doc.DocumentNode.Descendants("a")
                                                  .Select(a => a.GetAttributeValue("href", null))
                                                  .Where(u => !String.IsNullOrEmpty(u));
                return linkedPages;
            }
            catch(System.Net.WebException wex)
            {
                Console.WriteLine("The remote name could not be resolved.");
                Console.WriteLine(wex);
            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong!");
                Console.WriteLine(e);
            }

            return Enumerable.Empty<string>();

            //Non-functional way
            //List<string> pageUncheckedUrl = new List<string>();
            //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            //{
            //    HtmlAttribute att = link.Attributes["href"];
            //    if (!String.IsNullOrEmpty(att.Value))
            //    {
            //        pageUncheckedUrl.Add(att.Value);
            //    }               
            //}

            

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