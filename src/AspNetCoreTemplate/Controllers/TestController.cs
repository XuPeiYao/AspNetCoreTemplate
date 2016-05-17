using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTemplate.Extensions.AspNetCore;

namespace AspNetCoreTemplate.Controllers {
    public class TestController : Controller {
        public JsonResult Test() {
            string result = null;
            HttpContext.Session.TryGetStringValue("d", out result);
            HttpContext.Session.Set("d", "ggggg");
            return new JsonResult(result);
        }
    }
}