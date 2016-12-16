﻿// Please note that the following line must be incorporated at the beginning of every web page
// where the google maps api needs to be utilized.

// <script src="https://maps.googleapis.com/maps/api/js?libraries=places&callback=initialize"
//            async defer>
// </script>


// Please note that the following line must be incorporated onto every web page
// where the google map api needs to be utilized.

// <div id="map">
// </div>

var markers = [];
var active_markers = [];
var levelcount;
var marker_next_lvl = [];
//var markerpolylinealreadycreated = [];
var marker;
var latlngsent;
var latlngrcvd;
var linepaths = [];
var x = 0;
var y = 0;


function initialize() {
    var directionsService = new google.maps.DirectionsService();
    var directionsDisplay = new google.maps.DirectionsRenderer();
    var pittsburgh = new google.maps.LatLng(40.421796, -79.994485);
    mapOptions = { zoom: 13, mapTypeId: google.maps.MapTypeId.ROADMAP, center: pittsburgh }
    map = new google.maps.Map(document.getElementById("map"), mapOptions);
    directionsDisplay.setMap(map);

    //if (document.getElementById("searchAddressField")) {
    //    var input = document.getElementById('searchAddressField');
    //    var options = { componentRestrictions: { country: 'us' } };

    //    new google.maps.places.Autocomplete(input, options);
    //}
};

//Handles marker placement, dynamic zoom, and on click functionality.
function markerlocation(organizationid, address, organization, organizationabout) {
    $.getJSON('http://maps.googleapis.com/maps/api/geocode/json?address=' + address + '&organization=' + organization + '&organizationabout=' + organization + '&sensor=false', null, function (data) {
        var addresslocation = data.results[0].geometry.location;
        var latlng = new google.maps.LatLng(addresslocation.lat, addresslocation.lng);
        linepaths.push({ lat: addresslocation.lat, lng: addresslocation.lng });
        var marker = new google.maps.Marker({
            position: latlng, //it will place marker based on the addresses, which they will be translated as geolocations.
            map: map,
            title: organization,
            animation: google.maps.Animation.DROP,
            id: organizationid
        });        

        var contentString =
        '<div>' +
            '<div class="parentinfo">' +
                '<div class="logoholder">' + '<img src=/logos/' + organizationid + '/serve style= "width: 100%">' +
                '</div>' +
                '<p class="orgtitle">' + organization + '</p>' +
                '<p class="aboutlink">' + '<a href=/organizations/' + organizationid + '>' + 'about' + '</a>' + '</p>' +
            '</div>' +
            '<div class="descripcontainer">' +
                '<p class="descripbox">' + organizationabout + '</p>' +
            '</div>' +
        '</div>'
        ;

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        markers.push(marker);

        marker.addListener('click', function () {
            infowindow.open(map, marker);
            map.setCenter(marker.getPosition());
        });
    });
};

function createLine() {
    console.log("getting called");
    var lol = [{ lat: 40.421572, lng: -79.99410619999999 }, { lat: 40.4369999, lng: -79.9970905 }, { lat: 40.4218834, lng: -79.9939074 }];
    var lolly = linepaths.toString();
    console.log(lolly);
    flightPath = new google.maps.Polyline({
        path: linepaths,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2,
        map: null
    });

    //addLine();
    console.log("bottom of function");
}

function addLine() {
    flightPath.setMap(map);
}

function removeLine() {
    flightPath.setMap(null);
}

