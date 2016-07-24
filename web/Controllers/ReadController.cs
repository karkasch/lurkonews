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

namespace Lurkonews.Controllers
{
    public class ReadController : Controller
    {
        //private Lurkdict _lurk;
        // GET: Read
        [ValidateInput(false)]
        public ActionResult Index(string url)
        {
            //var url = "http://google.com?" + id;
            //var url = id;
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

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var baseUrl = request.RequestUri.GetLeftPart(UriPartial.Authority);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
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

                response.Close();
                readStream.Close();
            }

            // replace
            data = data.Replace(" src=\"/", " src=\"" + baseUrl + "/");
            data = data.Replace(" href=\"/", " href=\"" + baseUrl + "/");


            data = Lurkdict.Process(data);
            //data = data.Replace("Мутко", "Рашен Федерешен политишен");

            //Response.Write(data);
            return Content(data);
        }
    }
}