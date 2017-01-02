/**
 * Created by nadezda.salauyova on 29/12/2016.
 */
(function ($) {
    if (!$.Generator) {
        $.Generator = {};
    }

    if (!$.Generator.Points) {
        $.Generator.Points = {};
    }

    $.Generator.Points.mapPoints = function () {
        var base = this;
        var pointsList = [];

        var createMarker = function (point,html) {
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
        };

        base.getPints = function() {
            return pointsList;
        };

        base.init = function (map) {

            google.maps.event.addListener(map, 'click', function(event) {
                console.log('lat:' + event.latLng.lat() + ";  lng: " + event.latLng.lng());
                createMarker(event.latLng,
                                    'lat:' + event.latLng.lat()
                                    + "; </br> lng: " + event.latLng.lng() + ";");
                pointsList.push({
                    id: pointsList.length+1,
                    point: event.latLng                    
                });
                infowindow.close();
            });

        };

    };

    $.extend($.Generator.Points, {
        Points: $.Generator.Points.mapPoints()
    });
})(jQuery);