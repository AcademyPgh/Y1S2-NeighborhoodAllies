// Please note that the following line must be incorporated at the beginning of every web page
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
    mapOptions = { zoom: 10, mapTypeId: google.maps.MapTypeId.ROADMAP, center: pittsburgh }
    map = new google.maps.Map(document.getElementById("map"), mapOptions);
    directionsDisplay.setMap(map);

    //if (document.getElementById("searchAddressField")) {
    //    var input = document.getElementById('searchAddressField');
    //    var options = { componentRestrictions: { country: 'us' } };

    //    new google.maps.places.Autocomplete(input, options);
    //}
}

//Handles marker placement, dynamic zoom, and on click functionality.
function markerlocation(organizationid, address, organization, organizationabout) {
    y = 0;
    $.getJSON('http://maps.googleapis.com/maps/api/geocode/json?address=' + address + '&organization=' + organization + '&organizationabout=' + organization + '&sensor=false', null, function (data) {
        var addresslocation = data.results[0].geometry.location;
        var latlng = new google.maps.LatLng(addresslocation.lat, addresslocation.lng);
        var marker = new google.maps.Marker({
            position: latlng, //it will place marker based on the addresses, which they will be translated as geolocations.
            map: map,
            title: organization,
            animation: google.maps.Animation.DROP,
            id: organizationid
        });

        markers.push(marker);

        marker.addListener('click', function () {
            map.setCenter(marker.getPosition());
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

        marker.infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        // this is for styling the infowindow
        google.maps.event.addListener(marker.infowindow, 'domready', function () {

            // Reference to the DIV which receives the contents of the infowindow using jQuery
            var iwOuter = $('.gm-style-iw');

            /* The DIV we want to change is above the .gm-style-iw DIV.
             * So, we use jQuery and create a iwBackground variable,
             * and took advantage of the existing reference to .gm-style-iw for the previous DIV with .prev().
             */
            var iwBackground = iwOuter.prev();

            // Remove the background shadow DIV
            iwBackground.children(':nth-child(2)').css({ 'display': 'none' });

            // Remove the white background DIV
            iwBackground.children(':nth-child(4)').css({ 'display': 'none' });
            //console.log(iwBackground);

            // adds the gm-arrow-shadow class to the div used to make that shadow 
            //iwBackground.children(':nth-child(1)').attr('class', function(i,s){ return s + 'gm-arrow-shadow'});

            // Moves the arrow 76px to the left margin 
            //iwBackground.children(':nth-child(3)').attr('style', function(i,s){ return s + 'left: 76px !important;'});

            var iwCloseBtn = iwOuter.next();

            // Apply the desired effect to the close button
            iwCloseBtn.css({
                // by default the close button has an opacity of 0.7
                'right': '25px', 'top': '25px', // button repositioning
                // increasing button border and new color
                'border-radius': '1px', // circular effect
                'box-shadow': '0 0 1px #000',// 3D effect to highlight the button
            });

        });

        google.maps.event.addListener(marker, 'click', function () {
            infowindowclose();

            if (document.getElementById("viewall").checked == true) {
                document.getElementById("viewall").checked = false;
            }

            if ($('#button4').is(':checked')) {
                $("#button4").click();
            };

            marker.infowindow.open(map, marker);
            $(":radio[value=" + marker.id + "]").attr('checked', true);
            removeallpolylines();
            markerremoveall(marker);
            active_markers = [];
            linepaths = [];
            markerpolyline(organizationid, marker, function () {
                createsecondlevelmarkers();
            });
        });

        var latlngbounds = new google.maps.LatLngBounds();
        for (var i = 0; i < markers.length; i++) {
            latlngbounds.extend(markers[i].getPosition());
        }
        map.fitBounds(latlngbounds);

        return marker;
    });
}

function addpolyline(data, marker) {
    var sendrcvds = data; // array of arrays that include from lat/long, to lat/long, and the funding type (should be 5 values in each line's array)
    for (var i = 0; i < sendrcvds.length; i++) {
        var sendrcvd = sendrcvds[i];

        var line = { to: { lat: parseFloat(sendrcvd[0]), lng: parseFloat(sendrcvd[1]) }, from: { lat: parseFloat(sendrcvd[2]), lng: parseFloat(sendrcvd[3]) }, type: sendrcvd[4], amount: sendrcvd[5] };

        //for (var j = 0; j < sendrcvd.length; j++) { // why loop this? each item in this array has a specific value/meaning

        if (document.getElementById("fsupport").checked == true || document.getElementById("ksupport").checked == true) {

            /*if (j % 2 == 0) {
                var latitude = sendrcvd[j];
            } else {
                var longitude = sendrcvd[j];
            */
            var colorChoice = "";

            if (line.type == 1) {
                colorChoice = "#FF0000";
            } else {
                colorChoice = "#00FF00";
            }

            if ((document.getElementById("ksupport").checked == true && line.type == 1) || (document.getElementById("fsupport").checked == true && line.type == 2)) {
                /*var addressLocationObject = {lat: parseFloat(latitude), lng: parseFloat(longitude)};
                if ((j+1) % 4 != 0) {
                    latlngrcvd = addressLocationObject;
                } else {
                    latlngsent = addressLocationObject;
                }
                */
                var weight = 1
                if (line.amount < 50000) {
                    weight = 2;
                }
                else if (line.amount < 100000) {
                    weight = 3;
                }
                else if (line.amount < 250000) {
                    weight = 4;
                }
                else {
                    weight = 5;
                }
                //if ((j+1) % 4 == 0) {
                var linemapping = [
                    line.from, line.to
                ];

                var lineSymbol = {
                    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
                };

                var linepath = new google.maps.Polyline({
                    path: linemapping,
                    geodesic: true,
                    strokeColor: colorChoice,
                    strokeOpacity: 1.0,
                    strokeWeight: weight,
                    icons: [{
                        icon: lineSymbol,
                        offset: '100%'
                    }]
                });

                linepath.setMap(map);
                linepaths.push(linepath);
                markershow(line.from, line.to);
                //}
                //}
            }
        }
        //}
    }

    active_markers.push(marker);
}

function createsecondlevelmarkers() {
    if (document.getElementById("network").checked == true) {
        marker_next_lvl = [];
        for (var k = 0; k < active_markers.length; k++) {
            marker_next_lvl.push(active_markers[k]);
        };

        for (var l = 0; l < marker_next_lvl.length; l++) {
            markerpolyline(marker_next_lvl[l].id, marker_next_lvl[l]);
        };
        createthirdlevelmarkers();
    };
}

function createthirdlevelmarkers() {
    if (document.getElementById("network").checked == true) {
        for (var k = 0; k < active_markers.length; k++) {
            marker_next_lvl.push(active_markers[k]);
        };

        for (var l = 0; l < marker_next_lvl.length; l++) {
            markerpolyline(marker_next_lvl[l].id, marker_next_lvl[l]);
        };
    };
}

function infowindowclose() {
    for (var i = 0; i < markers.length; i++) {
        markers[i].infowindow.close();
    }
}

function mapdynamiczoom() {
    var latlngbounds = new google.maps.LatLngBounds();
    for (var i = 0; i < active_markers.length; i++) {
        latlngbounds.extend(active_markers[i].getPosition());
    }
    map.fitBounds(latlngbounds);
}

function markeridentify() {
    if (document.getElementById("viewall").checked == true) {
        document.getElementById("viewall").checked = false;
    }

    var ele = document.getElementsByName("orgselect");

    for (var i = 0; i < ele.length; i++) {
        if (ele[i].checked) {
            organizationid = ele[i].value

            for (var i = 0; i < markers.length; i++) {
                if (markers[i].id == organizationid) {
                    markers[i].setMap(map);
                    google.maps.event.trigger(markers[i], "click", {});
                }
            }
        }
    };
}

function markerpolyline(organizationid, marker, addsecondlevel) {
    $.post("organizations/" + marker.id + "/showvectors", function (data) {
        //$( ".result" ).html( data );
        addpolyline(data, marker);
        if (addsecondlevel)
        { addsecondlevel(); }
        mapdynamiczoom();
    });
}

function markerremoveall(marker) {
    for (var i = 0; i < markers.length; i++) {
        if (markers[i] != marker) {
            markers[i].setMap(null);
        }
    }
}

//Handles markers displayed based on the marker selected.
function markershow(latlngsent, latlngrcvd) {

    for (var i = 0; i < markers.length; i++) {
        if ((round(markers[i].getPosition().lat(), 6) == round(latlngrcvd.lat, 6) || round(markers[i].getPosition().lat(), 6) == round(latlngsent.lat, 6)) && (round(markers[i].getPosition().lng(), 6) == round(latlngrcvd.lng, 6) || round(markers[i].getPosition().lng(), 6) == round(latlngsent.lng, 6))) {
            markers[i].setMap(map);
            active_markers.push(markers[i]);
        }
    }
}

function markershowall() {
    if (viewall.checked == 1) {
        var latlngbounds = new google.maps.LatLngBounds();

        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
            latlngbounds.extend(markers[i].getPosition());
        }

        map.fitBounds(latlngbounds);
    }
}

function removeallpolylines() {
    for (var i = 0; i < linepaths.length; i++) {
        linepaths[i].setMap(null);
    }
}

function removeselectedradiobutton() {
    var ele = document.getElementsByName("orgselect");

    for (var i = 0; i < ele.length; i++) {
        ele[i].checked = false;
    };

    document.getElementById("fsupport").checked = false;
    document.getElementById("ksupport").checked = false;
    document.getElementById("network").checked = false;
}

function round(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}