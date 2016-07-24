using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lurkonews.Domain
{
    public class FileManager
    {
        public string Read()
        {
            var fileName = HttpContext.Current.Server.MapPath("~/data-1.txt");

            using(var streamReader = new StreamReader(fileName))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}