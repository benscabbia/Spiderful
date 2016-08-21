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
        public IEnumerable<string> GetLinks(string url, int level = 0, bool isOnSite = false, bool validatePages = false)
        {


            var linksRoot = UrlService.getPages(url, isOnSite, validatePages);

            
            return linksRoot; //+ UrlService.isValidSmart(url);

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
