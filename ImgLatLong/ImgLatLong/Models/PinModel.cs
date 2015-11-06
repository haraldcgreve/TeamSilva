using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgLatLong.Models
{
    public class PinModel
    {
        public string EventName { get; set; }

        public string LocationName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}