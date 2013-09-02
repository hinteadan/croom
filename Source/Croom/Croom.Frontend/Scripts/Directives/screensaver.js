(function () {
    "use strict";

    var screensaver = function (http) {

        this.restrict = 'EA';
        this.replace = true;
        this.templateUrl = 'Views/Templates/screensaver.html';
        this.scope = true;
        this.link = function (sc, el) {
            scope = sc;
            element = el;
            updateScreensaverInfo();
            run();
        }

        var timeout,
            image,
            recentImages = new Array(10),
            element,
            scope;

        function run() {
            attachEvents();
            fetchRandomImage();
        }

        function attachEvents() {
            $(document).on("mousemove keydown click", function () {
                resetTimeout(60000);
                hideScreensaver();
            }).click();
        }

        function resetTimeout(timeoutTime) {
            clearTimeout(timeout);
            timeout = setTimeout(function () {
                displayScreensaver();
                updateScreensaverInfo();
            }, timeoutTime);
        }

        function updateScreensaverInfo(){
            scope.currentTime = moment().format('HH:mm')
            setRoomInfo();
            if (!scope.$$phase) {
                scope.$apply();
            }
        }

        function setRoomInfo(){
            http.get('/Croom.Backend/Status').success(function (data) {
                if (data && data.IsOccupied) {
                    scope.roomStatus = "Occupied";
                    scope.roomDetails = data.CurrentReservation.Title + "\n" + data.CurrentReservation.Description;
                }
                else {
                    scope.roomStatus = "Available";
                    scope.roomDetails = null;
                }
            });
        }

        function displayScreensaver() {
            animatePhotos();
            showScreensaver();
            resetTimeout(10000);
            fetchRandomImage();
        }

        function animatePhotos() {
            var images = element.find('.screensaver-images'),
                visibleImg = images.find('.current-image'),
                hiddenImg = images.find(':not(.current-image)');

            if (!image || (image.src.indexOf("Content/dummyImg.JPG") != -1)) {
                image = new Image();
                image.src = "Content/dummyImg.JPG"
                visibleImg.attr('src', image.src);
                return;
            }
            hiddenImg.attr('src', image.src);
            visibleImg.fadeOut("slow");
            hiddenImg.fadeIn('slow');
            visibleImg.removeClass('current-image');
            hiddenImg.addClass('current-image');
        }

        function showScreensaver() {
            element.addClass('screensaver-active');
            element.removeClass('screensaver-disabled');
        }

        function hideScreensaver() {
            element.addClass('screensaver-disabled');
            element.removeClass('screensaver-active');
        }

        function fetchRandomImage() {
            var url = "http://picasaweb.google.com/data/feed/base/user/:user_id/albumid/:album_id?alt=json&kind=photo&hl=en_US&fields=entry(title,gphoto:numphotos,media:group(media:content,media:thumbnail))&imgmax=1600u";

            //[TODO-VLAD]: replace this with actual google plus/picasa user id,album id
            url = url.replace(/:user_id/, '102916204430573119519').replace(/:album_id/, '5910944160374225217');

            var images = new Array();

            $.getJSON(url, function (data) {
                $.each(data.feed.entry, function (i, element) {
                    images.push(element["media$group"]["media$content"][0]);
                });
                var random = Math.floor(Math.random() * data.feed.entry.length);
                while(_.contains(recentImages, images[random].url)) {
                    random = Math.floor(Math.random() * data.feed.entry.length);
                }
                image = new Image();
                image.src = images[random].url;
                recentImages.push(image.src);
                recentImages.shift();
            });
        }
    }

    var app = angular.module('Croom');
    app.directive('screensaver', ['$http', function ($http) {
        return new screensaver($http);
    }])

}).call(this);