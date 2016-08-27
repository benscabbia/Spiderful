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
        //    /api/sitemap?url=http://www.google.com/index.net&level=15&hostMatch=false
        public IEnumerable<string> GetLinks(string url, bool hostMatch = true, bool validatePages = true, int level = 0)
        {


            var linksRoot = UrlService.getLinks(url, hostMatch, validatePages, level);


            return linksRoot; //+ UrlService.isValidSmart(url);

        }

        //toTest as resetful, conflict with one above
        public string GetPage(string url)
        {
            var page = UrlService.getPage(url);
            return page;
        }

        public string GetPageWithSelector(string url, string selector, int index=0)
        {
            var page = UrlService.getPageWithSelector(url, selector, index);
            //https://untappd.com/search?q=leffe, //span[@class='num']
            //$(".num").first().text() and get value

            return page;
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
