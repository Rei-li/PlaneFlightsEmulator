/**
 * Created by nadezda.salauyova on 28/12/2016.
 */
var mArray = Array();
var map;
var centerPoint = new google.maps.LatLng(40.078071,-101.689453);
var rutedata ="zz33.760882,-84.38598633zz37.56199695,-77.4206543zz";
var points = new Array();
var coordinates = new Array();
var landmarks = [];
var polyline = null;
var map = null;
var bounds = null;
var infowindow = new google.maps.InfoWindow(
    {
        size: new google.maps.Size(150,50)
    });


/**
 *
 * 	Общепринятым считается следующее правило:
 * авиалайнеры, летящие на восток, юго-восток и северо-восток движутся на нечетных высотах (9 и 11 тысяч метров);
 * борта, летящие на запад, северо-запад и юго-запад движутся на четных (10-12 тысяч метров).
 * Макс. крейсерская скорость (км/ч)	795
 * Максимальная скорость (км/ч)	910
 * скорость посадки  188-196 км/ч
 * скорость взлета 737 – 220 км/ч
 *
 *
 * посадка
 500сек
 75 м/с
 500 м
 интервал 5сек , - 5 м , delta h / 100, delta V / 100


 взлет
 250сек
 500м
 интервал 5сек , -100м , delta h / 100, delta V / 100
 *
 **/


var resultsToWrite = [];
var pointsList = [];



function load() {
    doLoad();
    loaddata();

}

function doLoad() {
    var myOptions = {
        zoom: 7,
        center: new google.maps.LatLng(56.1, 10.7),
        mapTypeControl: true,
        mapTypeControlOptions: {style: google.maps.MapTypeControlStyle.DROPDOWN_MENU},
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas"),
        myOptions);

    google.maps.event.addListener(map, 'click', function(event) {
        console.log('lat:' + event.latLng.lat() + ";  lng: " + event.latLng.lng());
        var elevator = new google.maps.ElevationService;

        elevator.getElevationForLocations({
            'locations': [ event.latLng]
        }, function(results, status) {

            if (status === google.maps.ElevationStatus.OK) {
                // Retrieve the first result
                if (results[0]) {
                    createMarker(event.latLng, 'lat:' + event.latLng.lat() + ";  lng: " + event.latLng.lng() + '</br> The elevation at this point is ' + results[0].elevation + ' meters.');
                    console.log('The elevation at this point is ' +
                        results[0].elevation + ' meters.');

                    pointsList.push()

                } else {
                    console.log('No results found');
                }
            } else {
                console.log('Elevation service failed due to: ' + status);
            }
        });


        infowindow.close();
    });
    bounds = new google.maps.LatLngBounds();

}

function createMarker(point,html) {
    var contentString = html;
    var marker = new google.maps.Marker({
        position: point,
        map: map,
        title: name,
        zIndex: Math.round(point.lat()*-100000)<<5
    });

    google.maps.event.addListener(marker, 'click', function() {
        infowindow.setContent(contentString);
        infowindow.open(map,marker);
    });
    return marker;
}


function km_on() {
    if (landmarks.length && (landmarks.length > 0)) {
        for (var i=0; i<landmarks.length; i++) landmarks[i].setMap(map);
    } else {
        for (var i=0; i<polyline.Distance(); i+=2000) {
            var km_point = polyline.GetPointAtDistance(i);
            if (km_point) {


                console.log('lat:' + km_point.lat() + ";  lng: " + km_point.lng());
                addPointToResults(i, km_point);






                // var infoWindowContent = 'lat:' + km_point.lat() + ";  lng: " + km_point.lng();
                //     // "marker "+i/2000+" of "+Math.floor(polyline.Distance()/2000)+"<br>kilometer "+i/1000+" of "+(polyline.Distance()/1000).toFixed(2);
                // var landmark = createMarker(km_point, infoWindowContent);
                // landmarks.push(landmark);
            }
        }
    }
}


// function km_off() {
//     for (var i=0; i<landmarks.length; i++)
//         landmarks[i].setMap(null);
// }


function addPointToResults(id, point){

    var idSaved = id;
    var elevator = new google.maps.ElevationService;

    elevator.getElevationForLocations({
        'locations': [ point]
    }, function(results, status) {

        if (status === google.maps.ElevationStatus.OK) {
            // Retrieve the first result
            if (results[0]) {
                resultsToWrite.push({
                    id: idSaved,
                    lat: point.lat(),
                    lng: point.lng(),
                    alt: results[0].elevation
                });
            } else {
                console.log('No results found');
            }
        } else {
            console.log('Elevation service failed due to: ' + status);
        }
    });

}


function loaddata() {
    var pointsud = new Array();
    coordinates.lenght = 0;
    points = rutedata.split("zz");
    for (var i=1; i<(points.length-1); i++) {
        var mData = points[i].split(',');
        var point = new google.maps.LatLng(parseFloat(mData[0]),parseFloat(mData[1]));
        coordinates[ coordinates.length ] = [ point.lat(), point.lng() ];
        pointsud.push(point);
    }
    polyline = new google.maps.Polyline({
        path: pointsud,
        strokeColor: "#FF0000",
        strokeOpacity: 0.8,
        strokeWeight: 2
    });

    // topoDrawGraph( document.getElementById("graph"), coordinates, 950, 600 );
    polyline.setMap(map);

    var bounds = new google.maps.LatLngBounds();
    bounds = polyline.Bounds();
    map.fitBounds(bounds);

    //
    //
    // for (var i=0; i<polyline.Distance(); i+=2000) {
    //     var pointkm = polyline.GetPointAtDistance(i);
    //     if (pointkm) {
    //         // GLog.write(pointkm);
    //         //landmark = new GMarker(pointkm);
    //         //map.addOverlay(landmark);
    //     }
    // }


}



function download(name, type) {
    var a = document.getElementById("a");
    var text = JSON.stringify(resultsToWrite);
    var file = new Blob([text], {type: type});
    a.href = URL.createObjectURL(file);
    a.download = name;
}

function getRandomArbitrary(min, max) {
    return Math.random() * (max - min) + min;
}