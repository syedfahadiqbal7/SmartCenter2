﻿using Microsoft.AspNetCore.Mvc;
using static SCAPI.WebFront.Models.FileModel;


namespace SCAPI.WebFront.Controllers
{
    public class UploadController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UploadModel model)
        {
            var result = new ResultModel(model.File.FileName, model.File.Length, model.File.ContentType, MalwareStatus.Unknown, []);
            return View("Result", result);
        }
    }
}
