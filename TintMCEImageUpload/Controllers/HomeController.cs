using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TintMCEImageUpload.Models;

namespace TintMCEImageUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public HomeController(IHostingEnvironment _env, IConfiguration _configuration)
        {
            env = _env;
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, string FolderName = "Uploads")
        {
            if (file.Length > 0 && file.Length < 3000000)
            {
                if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg")
                {
                    string fileName;
                    string path = Path.GetFullPath(env.WebRootPath + "\\Content\\" + FolderName);
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    string resimAdi = "img-" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(file.FileName);
                    string dosyayolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Content\\" + FolderName + "\\" + resimAdi);
                    using (var fileStream = new FileStream(dosyayolu, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    fileName = "/Content/" + FolderName + "/" + resimAdi;
                    return Json(new { location = fileName });
                }
                else
                {
                    throw new Exception("<div class=\"alert alert-warning alert-dismissable\"><button type = \"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><h4><i class=\"icon fa fa-warning\"></i> Hata!</h4> Görsel dosya formatı geçersiz.</div>");
                }
            }
            else
            {
                throw new Exception("<div class=\"alert alert-warning alert-dismissable\"><button type = \"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><h4><i class=\"icon fa fa-warning\"></i> Hata!</h4> Görsel boyutu 3MB üzerinde olamaz.</div>");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
