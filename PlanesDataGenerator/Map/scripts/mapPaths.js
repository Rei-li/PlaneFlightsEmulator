/**
 * Created by nadezda.salauyova on 29/12/2016.
 */
(function ($) {
    if (!$.Generator) {
        $.Generator = {};
    }

    if (!$.Generator.Paths) {
        $.Generator.Paths = {};
    }

    $.Generator.Paths.mapPaths = function () {
        var base = this;
        var t = 60; // интервал в секундах
        var distanceUpDown = 500; // расстояние посадки и взлета в метрах
        var tUp = 250; // интервал взлета в секундах
        var tDown = 500; //  интервал посадки в секундах

        var V = 220; // максимальная скорость м/с

        var downVMin = 52.5; // минимальная скорость посадки м/с
        var downVMax = 54.45; // максимальная скорость посадки м/с

        var upnVMin = 61.1; // минимальная скорость взлета м/с
        var upVMax = 204.7; // максимальная скорость взлета м/с

        var heightMin = 9000; // минимальная высота полета
        var heightMax = 12000; // максимальная высота полета

        var downHeigh = 25;


        var getDistance = function(commonDistance, distance, speed){

            var planeUp = distance < distanceUpDown;
            var planeDown = (commonDistance - distance) < distanceUpDown;

            var deltaUpV = upnVMin/ 100;
            var deltaDownV = downVMin/ 100;

            if(planeUp  || ((distance > distanceUpDown) && speed < V) )
            {
                speed = speed + deltaUpV;
            }
            else if(planeDown   || (( (commonDistance - distance) < distanceUpDown*4) &&  speed > downVMin)){
                speed = speed - deltaDownV;
            }


            return {
                distance : speed * t,
                speed : speed
            };



        };

        var getHeight = function(commonDistance, distance, height, deltaHeight, restDistanceToDown){

            var planeUp = distance < distanceUpDown;
            var planeStabel = distance > distanceUpDown &&  (commonDistance - distance) > distanceUpDown;
            var planeDown = (commonDistance - distance) < distanceUpDown*4;




            var deltaH = deltaHeight;
            var downdDltaH = height/100;

            if(planeUp && height < heightMax)
            {
                height = height + deltaH;
            }
            else if(planeDown){
                if(height > downHeigh)
                {
                    height = downHeigh;
                }
                else{
                    height = height - deltaH;
                }
            }
            if(planeStabel )
            {


                var restDistance = commonDistance - distance;
                if (( restDistance < restDistanceToDown) &&  height > downHeigh){

                    height = height - deltaH;

                    if(height < downHeigh)
                    {
                        height = downHeigh;
                    }

                }
                else if(height < heightMin && ( restDistance > restDistanceToDown) ){
                    height = height + deltaH;
                }

            }


            if(height<0)
            {
                height = 0;
            }

            return height;



        };



        var addPointToResults = function(id, point, path, height){
            path.push({
                id: id,
                lat: point.lat(),
                lng: point.lng(),
                alt: height
            });
        };

        var getPath = function(polyline) {
            var pathPoints = [];
            var path = [];

            var pathLength =  google.maps.geometry.spherical.computeLength(polyline.getPath());
            var distancePassed = 0;
            var speed = 0;
            var speedDistance =  getDistance(pathLength, distancePassed, speed);
            var height = getHeight(pathLength, distancePassed, 0, speedDistance.distance);
            var restDistanceToDownSet = false;
            var restDistanceToDown = pathLength/2;


            for (var i=0; i<polyline.Distance(); i+= speedDistance.distance ) {
                speed = speedDistance.speed;
                distancePassed += speedDistance.distance;

                console.log("distancePassed: "+ distancePassed);
                console.log("speed: " + speed);
                var km_point = polyline.GetPointAtDistance(i);
                if (km_point) {
                    console.log('lat:' + km_point.lat() + ";  lng: " + km_point.lng());
                    // pathPoints.push(addPointToResults(i, km_point, path));
                    addPointToResults(i, km_point, path, height);
                }
                speedDistance =  getDistance(pathLength, distancePassed, speed);

                if(height >= heightMin && !restDistanceToDownSet ){
                    restDistanceToDown = distancePassed*2;
                    restDistanceToDownSet = true;
                }
                height = getHeight(pathLength, distancePassed, height, speedDistance.distance, restDistanceToDown);

            }
            // return Promise.all(pathPoints).then(() => {return path});

             return path;
        };


        var generatePath = base.generatePath = function(pointStart, pointEnd){

            return polyline = new google.maps.Polyline({
                path: [pointStart, pointEnd],
                strokeColor: "#FF0000",
                strokeOpacity: 0.8,
                strokeWeight: 2
            });

        };

        base.init = function (pointStart, pointEnd) {
            return getPath(generatePath(pointStart, pointEnd));
        };

    };

    $.extend($.Generator.Paths, {
        Paths: $.Generator.Paths.mapPaths()
    });
})(jQuery);