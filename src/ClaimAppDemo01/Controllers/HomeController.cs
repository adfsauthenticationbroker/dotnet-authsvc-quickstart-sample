using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace ClaimAppDemo01.Controllers
{
    public class HomeController : Controller
    {



        public ActionResult Index()
        {
            // form html
            ViewBag.Title = "Home Page" ;
            return View();
        }

    }
}
