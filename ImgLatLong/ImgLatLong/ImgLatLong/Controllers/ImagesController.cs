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

namespace ImgLatLong.Controllers
{
    public class ImagesController : Controller
    {

        string _Instagram_client_id = "833d878fa43c4f24b80a4631e50bfb1a";
        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string query)
        {
            ImageListViewModel model = null;
 
            if (!string.IsNullOrEmpty(query))
            {
                decimal lat = 0;
                decimal lng = 0;

                string urigeo = string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}", query);

                var result = new WebClient().DownloadString(urigeo);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var geoJson = jss.Deserialize<dynamic>(result);

                if (geoJson != null)
                {
                    lat = geoJson["results"][0]["geometry"]["location"]["lat"];
                    lng = geoJson["results"][0]["geometry"]["location"]["lng"];
                }



                //Get the images agains the lat long from instagram 


                string uriInstagram = string.Format(@"https://api.instagram.com/v1/media/search?lat={0}&lng={1}&client_id={2}", lat.ToString(), lng.ToString(), _Instagram_client_id);

                var InstaResult = new WebClient().DownloadString(uriInstagram);
                JavaScriptSerializer Istajss = new JavaScriptSerializer();
                var instaJson = Istajss.Deserialize<dynamic>(InstaResult);

                var data = instaJson["data"];

                List<ImageModel> imgList = new List<ImageModel>();



                foreach (var img in data)
                {

                    imgList.Add(new ImageModel { ImageUrl = img["images"]["low_resolution"]["url"] });


                }

                model = new ImageListViewModel { ImageList = imgList };

                //Pass the images to the view to show on the screen.
            }
            else
            {
                model.Message = "Please type in the city name";

            }


            return View(model);
        }
    }
}