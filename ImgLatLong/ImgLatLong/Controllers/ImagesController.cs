using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using ImgLatLong.Models;
using Microsoft.Owin.Security.Twitter.Messages;

namespace ImgLatLong.Controllers
{
    public class ImagesController : Controller
    {

        string _Instagram_client_id = "833d878fa43c4f24b80a4631e50bfb1a";
        private const string AccessToken = "2233692468.97d3404.d6f0d30225504e978d424b8db1091997";
        private double plusminusLatLng = 0.01;
        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string query)
        {
            ImageListViewModel model = GetImageList(query);

            return View(model);
        }

        private ImageListViewModel GetImageList(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var coordinates = GetLatLngFromLocation(query);



                string uriInstagram =
                    string.Format("https://api.instagram.com/v1/tags/teammacmillan/media/recent?access_token={0}",
                        AccessToken);

                var result = new WebClient().DownloadString(uriInstagram);

                JavaScriptSerializer jss = new JavaScriptSerializer();

                var instaJson = jss.Deserialize<dynamic>(result);

                var data = instaJson["data"];

                List<ImageModel> imgList = new List<ImageModel>();

                foreach (var img in data)
                {
                    if (img["location"] != null)
                    {

                        var lat = (double)img["location"]["latitude"];
                        var lng = (double)img["location"]["longitude"];

                        if (lat + plusminusLatLng > coordinates[0]
                            && lat - plusminusLatLng < coordinates[0]
                            && lng + plusminusLatLng > coordinates[1]
                            && lng - plusminusLatLng < coordinates[1])
                        {
                            var name = img["location"]["name"];

                            imgList.Add(new ImageModel
                            {
                                ImageUrl = img["images"]["low_resolution"]["url"],
                                Latitude = lat,
                                Longitude = lng,
                                Name = name
                            });
                        }
                    }
                }

                return new ImageListViewModel { ImageList = imgList };
            }
            return new ImageListViewModel();
        }



        public ActionResult GetImageJson(string query)
        {
            ImageListViewModel model = GetImageList(query);

            return Json(model, JsonRequestBehavior.AllowGet);

            //return Content(new JavaScriptSerializer().Serialize(model));
        }

        public ActionResult GetImageJsonForLatLng(string lat, string lng)
        {
            return null;
        }

        private List<double> GetLatLngFromLocation(string query)
        {
            string urigeo = string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}", query);

            var result = new WebClient().DownloadString(urigeo);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var geoJson = jss.Deserialize<dynamic>(result);

            List<double> latLongs = new List<double>();

            if (geoJson != null)
            {
                latLongs.Add((double)geoJson["results"][0]["geometry"]["location"]["lat"]);
                latLongs.Add((double)geoJson["results"][0]["geometry"]["location"]["lng"]);
            }

            return latLongs;
        }
    }
}