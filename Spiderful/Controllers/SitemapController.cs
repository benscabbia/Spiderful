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
        public string Get(string url, int level=0, bool isOnSite=true)
        {

            //

 
            //string a = "www.google.com";
            //bool b = testc("google.com");
            //string a = "http://google.com";
            //bool d = testc("http://google.com");
            //bool e = testc("lanext.it");

            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return "result: " + result;
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
