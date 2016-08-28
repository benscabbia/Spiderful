using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

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

        /// <summary>
        /// A method which returns all pages for a given url
        /// </summary>
        /// <param name="url">A strinct containing the URL to parse</param>
        /// <param name="hostMatch">A boolean determing whether returned result should contain pages containing host</param>
        /// <param name="validatePages">A boolean determing whether returned result should contain pages that have been validated</param>
        /// <returns>A collection of pages</returns>
        public static IEnumerable<string> getLinks(string url, bool hostMatch=true, bool validatePages=true, int level=0)
        {
            //make sure url is ok
            string formattedUrl = urlFormatValidator(url);
            
            //if formattedUrl is empty, url is broken
            if (string.IsNullOrEmpty(formattedUrl)) return Enumerable.Empty<string>();

            //always get root urls first
            IEnumerable<string> rootUrls = getSinglePageLinks(formattedUrl, hostMatch, validatePages);

            for (int i=0; i<level; i++)
            {
                rootUrls = rootUrls.Union(getManyPageLinks(rootUrls, hostMatch, validatePages));
            }

            return rootUrls;
        }

        private static IEnumerable<string> getSinglePageLinks(string formattedUrl, bool hostMatch = true, bool validatePages = true)
        {
            try
            {
                HtmlDocument doc = new HtmlWeb().Load(formattedUrl);
                //var linkTags = doc.DocumentNode.Descendants("link");
                var linkedPages = doc.DocumentNode.Descendants("a")
                                                  .Select(a => a.GetAttributeValue("href", null))
                                                  .Where(u => !String.IsNullOrEmpty(u))
                                                  .Distinct();

                IEnumerable<string> linkedPagesHost = null, linkedPagesValidated = null, linkedPagesValidatedHost = null;

                if (hostMatch)
                {
                    var urlHost = new Uri(formattedUrl).Host;
                    if (urlHost.Substring(0, 4).Equals("www.")) urlHost = urlHost.Substring(4);
                    //var linkedPagesHost = linkedPages.Where(a => urlHost.All(a.Contains));
                    linkedPagesHost = linkedPages.Where(a => a.Contains(urlHost));
                    if (!validatePages) return linkedPagesHost;
                }

                if (validatePages)
                {
                    linkedPagesValidated = linkedPages.Where(p => isValid(p) == true);
                    if (!hostMatch) return linkedPagesValidated;
                }

                if (hostMatch && validatePages)
                {
                    linkedPagesValidatedHost = linkedPagesHost.Intersect(linkedPagesValidated);
                    return linkedPagesValidatedHost;
                }
                return linkedPages;
            }
            catch (System.Net.WebException wex)
            {
                Console.WriteLine("The remote name could not be resolved.");
                Console.WriteLine(wex);
            }
            catch (Exception e)
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

        private static IEnumerable<string> getManyPageLinks(IEnumerable<string> rootUrls, bool hostMatch, bool validatePages)
        {
            List<Task> tasks = new List<Task>();
            List<List<string>> allLinks = new List<List<string>>();

            foreach (string rootUrl in rootUrls)
            {
                string rootUrlCopy = rootUrl; //http://stackoverflow.com/questions/4684320/starting-tasks-in-foreach-loop-uses-value-of-last-item
                var task = Task.Factory.StartNew(() =>
                {
                    IEnumerable<string> taskResult = getSinglePageLinks(rootUrlCopy, hostMatch, validatePages);
                    return taskResult;
                });

                //task.ContinueWith(t => t.Result);
                tasks.Add(task);
                allLinks.Add(task.Result.ToList());
            }
            //https://msdn.microsoft.com/en-us/library/dd537609.aspx

            Task.WaitAll(tasks.ToArray());
            return allLinks.SelectMany(x => x).Distinct();            

        }

        public static string getPage(string url)
        {
            string formattedUrl = urlFormatValidator(url);
            if (string.IsNullOrEmpty(formattedUrl)) return "";
            HtmlDocument doc = new HtmlWeb().Load(formattedUrl);
            return doc.DocumentNode.OuterHtml;
        }

        public static string getPageWithSelector(string url, string selector, int index=0)
        {
            try
            {
                string formattedUrl = urlFormatValidator(url);
                if (string.IsNullOrEmpty(formattedUrl)) return "";
                HtmlDocument doc = new HtmlWeb().Load(formattedUrl);
                ////div[@class='bla'] or //span[@class='num']
                var nodes = doc.DocumentNode.SelectNodes(selector);
                var firstNode = nodes[index];
                return firstNode.InnerHtml; //first node


            }
            catch(NullReferenceException e)
            {
                Console.WriteLine("The selector returned a null item");
                return "";
            }
            catch(Exception e)
            {
                return "";
            }
            //foreach (HtmlNode node in nodes)
            //{
            //    Console.WriteLine(node.OuterHtml);
            //    Console.WriteLine();
            //}
        }

        //testing alternative method
        async static Task<IEnumerable<string>> GetAllPagesLinks2(IEnumerable<string> rootUrls, bool hostMatch, bool validatePages)
        {
            var result = await Task.WhenAll(rootUrls.Select(url => GetPageLinks2(url, hostMatch, validatePages)));

            return result.SelectMany(x => x).Distinct();
        }

        static async Task<IEnumerable<string>> GetPageLinks2(string formattedUrl, bool hostMatch = true, bool validatePages = true)
        {
            var htmlDocument = new HtmlDocument();

            try
            {
                using (var client = new HttpClient())
                    htmlDocument.LoadHtml(await client.GetStringAsync(formattedUrl));

                var linkedPages = htmlDocument.DocumentNode
                                   .Descendants("a")
                                   .Select(a => a.GetAttributeValue("href", null))
                                   .Where(u => !string.IsNullOrEmpty(u))
                                   .Distinct();

                IEnumerable<string> linkedPagesHost = null, linkedPagesValidated = null, linkedPagesValidatedHost = null;

                if (hostMatch)
                {
                    var urlHost = new Uri(formattedUrl).Host;
                    if (urlHost.Substring(0, 4).Equals("www.")) urlHost = urlHost.Substring(4);
                    //var linkedPagesHost = linkedPages.Where(a => urlHost.All(a.Contains));
                    linkedPagesHost = linkedPages.Where(a => a.Contains(urlHost));
                    if (!validatePages) return linkedPagesHost;
                }

                if (validatePages)
                {
                    linkedPagesValidated = linkedPages.Where(p => isValid(p) == true);
                    if (!hostMatch) return linkedPagesValidated;
                }

                if (hostMatch && validatePages)
                {
                    linkedPagesValidatedHost = linkedPagesHost.Intersect(linkedPagesValidated);
                    return linkedPagesValidatedHost;
                }
                return linkedPages;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return Enumerable.Empty<string>();
            }
        }
        public async static Task<IEnumerable<string>> GetLinks2(string url, bool hostMatch = true, bool validatePages = true, int level = 0)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            string formattedUrl = urlFormatValidator(url);

            if (string.IsNullOrEmpty(formattedUrl))
                return Enumerable.Empty<string>();

            var rootUrls = await GetPageLinks2(formattedUrl, hostMatch, validatePages);

            if (level == 0)
                return rootUrls;

            var links = await GetAllPagesLinks2(rootUrls, hostMatch, validatePages);
            //broken! 
            var tasks = await Task.WhenAll(links.Select(link => GetLinks2(link, hostMatch, validatePages, --level)));

            //allLinks.Add(task.Result.ToList());

            return tasks.SelectMany(x => x);
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