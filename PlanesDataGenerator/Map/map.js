/**
 * Created by nadezda.salauyova on 28/12/2016.
 */
var centerPoint = new google.maps.LatLng(53.90433815627471,27.5592041015625);
var polyline = null;
var map = null;
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

var pathsCount = 10;
var resultsToWrite = [];
var pointsList = [];



function load() {
    doLoad();
    loaddata();

}

function doLoad() {
    var myOptions = {
        zoom: 7,
        center: centerPoint,
        mapTypeControl: true,
        mapTypeControlOptions: {style: google.maps.MapTypeControlStyle.DROPDOWN_MENU},
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas"),
        myOptions);
    $.Generator.Points.init(map);
}




function km_on() {
    var points = $.Generator.Points.getPints();
    var paths = [];

    for (var i=0; i<pathsCount; i++)
    {
        var startPoint = getRandomFromArray(points);
        var endPoint = getEndPoint(startPoint.id, points);
        paths.push(savePath(startPoint.point, endPoint.point));
    }
    return Promise.all(paths).then(() => {
        download('mapPoints.json', 'application/json');
        var a = $('#a').show();

    });

}

function savePath(startPoint, endPoint)
{
    $.Generator.Paths.generatePath(startPoint, endPoint).setMap(map);
    // $.Generator.Paths.init(startPoint, endPoint).then(path => { resultsToWrite.push(path)});
    var path =  $.Generator.Paths.init(startPoint, endPoint);
    resultsToWrite.push(path);
}

function getEndPoint(id, points){
    var endPoint = getRandomFromArray(points);

    if(endPoint.id != id ){
       return endPoint;
    }
    return getEndPoint(id, points);
}


function loaddata() {
    var a = $('#a').hide();
}



function download(name, type) {
    var a = document.getElementById("a");
    var text = JSON.stringify(resultsToWrite);
    var file = new Blob([text], {type: type});
    a.href = URL.createObjectURL(file);
    a.download = name;
}

function getRandomFromArray(arr) {
    return arr[Math.floor(Math.random() * arr.length)];
}