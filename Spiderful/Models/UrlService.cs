using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

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
        public static bool isValid(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        /// <summary>
        /// A method used to determine whether a URL is valid, regardless if it has a scheme. 
        /// Valid URL's include: www.test.com, test.ocm, test.com/page1/page2 and all of isValid
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool isValidSmart(string url)
        {
            if (isValid(url))
            {
                return true;
            }
            else
            {
                UriBuilder link = new UriBuilder(url);
                return isValid(link.Uri.ToString());
            }
        }

        public static Enumerable getUrls(int level=0)
        {

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