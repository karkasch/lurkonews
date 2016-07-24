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
    public class DictitController : Controller
    {
        //private Lurkdict _lurk;
        // GET: Read
        public ActionResult Index()
        {
            return Content("dew");
        }
    }
}