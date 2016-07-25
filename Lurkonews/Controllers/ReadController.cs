using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Lurkonews.Domain;
using System.Threading.Tasks;

namespace Lurkonews.Controllers
{
    public class ReadController : Controller
    {
        //private Lurkdict _lurk;
        // GET: Read
        [ValidateInput(false)]
        public async Task<ActionResult> Index(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    return Content(null);

                url = url.ToLower();
                if (!url.StartsWith("http"))
                    url = "http://" + url;

                var re = new Regex("http:/[a-z]+.+");
                if (re.IsMatch(url))
                    url = url.Replace("http:/", "http://");

                re = new Regex("https:/[a-z]+.+");
                if (re.IsMatch(url))
                    url = url.Replace("https:/", "https://");


                var uri = new Uri(url);


                string data = "";
                var baseUrl = new Uri(url).GetLeftPart(UriPartial.Authority);


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                
                using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    baseUrl = request.RequestUri.GetLeftPart(UriPartial.Authority);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var receiveStream = response.GetResponseStream())
                        {
                            StreamReader readStream = null;

                            if (response.CharacterSet == null)
                            {
                                readStream = new StreamReader(receiveStream, true);
                            }
                            else
                            {
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            data = readStream.ReadToEnd();
                        }
                    }
                }

                // replace
                data = Regex.Replace(data, @"( src=""/)([a-z]{1})", " src=\"" + baseUrl + "/$2");

                data = Regex.Replace(data, @"( href=""/)([a-z]{1})", " href=\"" + baseUrl + "/$2");

                var repRe = new Regex(@"<script[^>]*>(.*?)<\/script[^>]*>", RegexOptions.IgnoreCase & RegexOptions.Singleline);
                data = repRe.Replace(data, "");

                data = Lurkdict.Process(data);

                data += @"<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-68881913-2', 'auto');
  ga('send', 'pageview');

</script>";

                return Content(data);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
    }
}