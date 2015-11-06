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

        private List<PinModel> _events = new List<PinModel>();    
        
        
       
        private IEnumerable<PinModel> FetchPinData()
        {
            _events.Add(new PinModel
            {
                EventName = "Vauxhall Hack Day 2015",
                Latitude = 51.4886,
                Longitude = -0.1207,
                LocationName = "89 Albert Embankment, London"
            });

            _events.Add(new PinModel
            {
                EventName = "Hackney Empire Charity Concert",
                Latitude = 51.5333,
                Longitude = -0.0833,
                LocationName = "Hackney, London"
            });

            _events.Add(new PinModel
            {
                EventName = "Wimbledon Tennis Tournament",
                Latitude = 51.4235,
                Longitude = -0.2171,
                LocationName = "Wimbledon"
            });

            _events.Add(new PinModel
            {
                EventName = "Hyde Park Softball Tournament",
                Latitude = 51.5086,
                Longitude = -0.1636,
                LocationName = "Hyde Park, London"
            });

            _events.Add(new PinModel
            {
                EventName = "Wembley Charity Football Match",
                Latitude = 51.5560,
                Longitude = -0.3042,
                LocationName = "Wembley Stadium"
            });

            _events.Add(new PinModel
            {
                EventName = "Woking Half Marathon",
                Latitude = 51.3162,
                Longitude = -0.5610,
                LocationName = "Woking, Surrey"
            });

            _events.Add(new PinModel
            {
                EventName = "Olympic Park 10K Run",
                Latitude = 51.5462,
                Longitude = -0.0127,
                LocationName = "Olympic Park, Stratford"
            });

            return _events;

        }

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


                var imgList = GetImages(coordinates[0], coordinates[1]);
                

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
            ImageListViewModel model;

            var imgList = GetImages(Convert.ToDouble(lat), Convert.ToDouble(lng));

            return Json(new ImageListViewModel { ImageList = imgList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEventDataJson()
        {
            return Json(FetchPinData(), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<ImageModel> GetImages(double lati, double lngi)
        {
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

                    if (lat + plusminusLatLng > lati
                        && lat - plusminusLatLng < lati
                        && lng + plusminusLatLng > lngi
                        && lng - plusminusLatLng < lngi)
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

            return imgList;
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