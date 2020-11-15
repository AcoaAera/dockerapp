using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s3bucketapp.Helpers;
using s3bucketapp.Models;

namespace s3bucketapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAWSS3BucketHelper _AWSS3BucketHelper;
        public List<Item> RecognizedItems = new List<Item>();


        public HomeController(IAWSS3BucketHelper AWSS3BucketHelper)
        {
            this._AWSS3BucketHelper = AWSS3BucketHelper;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Recognize(string fileName)
        {
            ListItems.list.Clear();
            var detectList = _AWSS3BucketHelper.RecognizeImage(fileName);
            foreach (var item in detectList.Result.Labels)
            {
                Item item1 = new Item(item.Name, item.Confidence);
                ListItems.list.Add(item1);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
