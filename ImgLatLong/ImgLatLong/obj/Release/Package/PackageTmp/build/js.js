var mapModule = (function() {

  // GLOBAL VARS
  var map = L.map('map',{
    animate: true,
  });
  var currentLat;
  var currentLng;


  var init = function() {
    initMap();
    populateMarkers();
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
        map.panTo(new L.LatLng(currentLat, currentLng), {animate: true, duration: 1, zoom: 13});
        $('.logo img').css({'-webkit-filter': 'grayscale(0)'});
        // setTimeout(function(){ populateMarkers(); }, 2000);
      });
    });

    $('.close').on('click', function() {
      $('.gallery-panel').fadeOut(800, function(){
        $('.gallery').slick('unslick');
      });

    });
  }

  var getImages = function() {
    var location = this.getLatLng();
    var url = 'http://teamsilva.azurewebsites.net/Images/GetImageJsonForLatLng?lat=' + location.lat + '&lng=' + location.lng;

    $.ajax({
      url: url,
    }).done(function(data) {

      $('.gallery-panel').fadeIn(800).append(data);
      var images = data.ImageList;
      console.log(images);
      var markup;

      for (i=0; i < images.length; i++) {
        markup += '<div><img src="' + images[i].ImageUrl + '" /><p>' + images[i].Name + '</div>';
      }

      $('.gallery').html(markup).slick({
        prevArrow: '<span class="slick-prev slick-arrow">&lsaquo;</span>',
        nextArrow: '<span class="slick-next slick-arrow">&rsaquo;</span>',
      });
    });
  }

  var populateMarkers = function() {
    $.ajax({
      url: 'http://teamsilva.azurewebsites.net/Images/GetEventDataJson',
    }).done(function(data) {

      var imageList = data;

      for (i=0; i < imageList.length; i++) {
        var marker = L.marker([imageList[i].Latitude, imageList[i].Longitude])
        marker.addTo(map).on('click', getImages);
      }
    });

  }

  return {
    init: init,
  }

})();

mapModule.init();
