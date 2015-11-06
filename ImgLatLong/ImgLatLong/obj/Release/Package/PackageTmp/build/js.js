var mapModule = (function() {

  // GLOBAL VARS
  var map = L.map('map',{
    animate: true,
  });
  var currentLat;
  var currentLng;


  var init = function() {
    initMap();
    getLocation();
    eventHandlers();
  }

  var getLocation = function() {

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(showGeoData, errorCallback, { timeout: 10000 });
    }
  }
  function errorCallback(err) {
    console.log(err);
  }

  function showGeoData(geoData) {
      currentLat = geoData.coords.latitude;
      currentLng = geoData.coords.longitude;

      // $.ajax({
      //     url: 'http://api.postcodes.io/postcodes',
      //     type: 'GET',
      //     data: { lat: currentLat, lng: currentLng },
      //     success: function (data) {
      //
      //       console.log(data);
      //       currentLat = lat;
      //       currentLng = lng;
      //     }
      // });
  }

  var initMap = function() {
    var locationNYLat = 40.7127;
    var locationNYLng = 74.0059;

    var locationParisLat = 48.8567;
    var locationParisLng = 2.3508;

    map.setView([locationParisLat, locationParisLng], 8, {animate : false});

    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
      attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
      maxZoom: 18,
      id: 'wwong4macmillan.cigmdp4gs00h0wfknt3rsa9i4',
      accessToken: 'pk.eyJ1Ijoid3dvbmc0bWFjbWlsbGFuIiwiYSI6ImNpZ21kcDRtYTAwZjh3amt0eDlldGNhbnMifQ.B_0FAM2oX4eyWsZ7RidYQQ'
    }).addTo(map);
  }

  var eventHandlers = function() {
    var introPanel = $('.intro-panel');
    var introCta = introPanel.find('.cta');

    introCta.on('click', function() {
      introPanel.fadeOut(800, function() {
        map.panTo(new L.LatLng(currentLat, currentLng), {animate: true, duration: 4, zoom: 13});
        $('.logo img').css({'-webkit-filter': 'grayscale(0)'});
        // setTimeout(function(){ populateMarkers(); }, 2000);
        populateMarkers();
      });
    });

    $('.leaflet-clickable').on('click', function() {

    });
  }

  var populateMarkers = function() {
    $.ajax({
      url: 'http://teamsilva.azurewebsites.net/Images/GetEventDataJson',
    }).done(function(data) {
      console.log(data);
      var imageList = ImageList.ImageList;

      for (i=0; i < imageList.length; i++) {
        var marker = L.marker([imageList[i].Latitude, imageList[i].Longitude])
        marker.addTo(map);
      }
    });

  }

  return {
    init: init,
  }

})();

mapModule.init();

var ImageList = {
  "ImageList":[
    {
      "ImageUrl":"https://scontent.cdninstagram.com/hphotos-xpf1/t51.2885-15/s320x320/e35/11875507_1653712601567733_556205717_n.jpg",
      "Latitude":51.4874013,
      "Longitude":-0.124641122,
      "Name":"89 Albert Embankment, Vauxhall"
    },
    {
      "ImageUrl":"https://scontent.cdninstagram.com/hphotos-xpf1/t51.2885-15/s320x320/e35/11849882_842178569233812_2050902676_n.jpg",
      "Latitude":51.4874013,
      "Longitude":-0.124641122,
      "Name":"89 Albert Embankment, Vauxhall"
    },
    {
      "ImageUrl":"https://scontent.cdninstagram.com/hphotos-xft1/t51.2885-15/s320x320/e35/12063263_1508716202756008_1990820026_n.jpg",
      "Latitude":51.4903,
      "Longitude":-0.1193,
      "Name":"Vauxhall"
    },
    {
      "ImageUrl":"https://scontent.cdninstagram.com/hphotos-xap1/t51.2885-15/s320x320/e35/10848481_509297382579373_468583301_n.jpg",
      "Latitude":51.4874013,
      "Longitude":-0.124641122,
      "Name":"89 Albert Embankment, Vauxhall"
    },
    {
      "ImageUrl":"https://scontent.cdninstagram.com/hphotos-xfp1/t51.2885-15/s320x320/e35/11917880_137934793230197_1594623928_n.jpg",
      "Latitude":51.4903,
      "Longitude":-0.1193,
      "Name":"Vauxhall"
    }
  ],
  "Message":null
}
