using HtmlAgilityPack;
using Spiderful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Spiderful.Controllers
{
    public class SitemapController : ApiController
    {
        // GET api/sitemap
        public IEnumerable<string> Get()
        {
            //todo really, probably return usage page?
            return new string[] { "link1", "link2" };
        }

        // GET api/sitemap/5 if Get(int id)
        // GET api/sitemap?url=http://www.google.com for below
        //    /api/sitemap?url=http://www.google.com/index.net&level=15&isOnSite=false
        public string GetLinks(string url, int level=0, bool isOnSite=true)
        {
            //string formatted_url = UrlService.urlFormatValidator(url);

            string a = "http://www.google.com";
            string b = "google.com";
            string c = "homePage";
            //string formated1 = UrlService.urlFormatValidator(a);
            //string formated2 = UrlService.urlFormatValidator(b);
            string formated3 = UrlService.urlFormatValidator(c, a);

            
            return "result: "; //+ UrlService.isValidSmart(url);

            //http://stackoverflow.com/questions/2248411/get-all-links-on-html-page
            //https://htmlagilitypack.codeplex.com/downloads/get/120935#
            //https://htmlagilitypack.codeplex.com/
            //https://www.spotify.com/ie/legal/new-60-days-free-trial-terms-and-conditions/


            //string a = "www.google.com";
            //bool b = testc("google.com");
            //string a = "http://google.com";
            //bool d = testc("http://google.com");
            //bool e = testc("lanext.it");


        }

        // POST api/sitemap
        public void Post([FromBody]string value)
        {
        }

        // PUT api/sitemap/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/sitemap/5
        public void Delete(int id)
        {
        }
    }
}
