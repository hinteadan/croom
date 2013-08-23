(function () {
    "use strict";

    var screensaver = function (element) {

        var timeout;
        var image;
        var self = this;

        function run() {
            attachEvents();
            fetchRandomImage();
        }

        function attachEvents() {
            $(document).on("mousemove keydown click", function () {
                resetTimeout(10000);
                hideScreensaver();
            }).click();
        }

        function resetTimeout(timeoutTime) {
            clearTimeout(timeout);
            timeout = setTimeout(function () {
                displayScreensaver();
            }, timeoutTime);
        }

        function displayScreensaver() {
            element.addClass('screensaver-active');
            var backgroundImageUrl = "url(" + self.image.src + ")";
            element[0].style.background = backgroundImageUrl;
            resetTimeout(5000);
            fetchRandomImage();
        }
        function hideScreensaver() {
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
                self.image = new Image();
                self.image.src=images[random].url;
            });
        }

        return {
            run: run
        };
    }

    var app = angular.module('Croom');
    app.directive('screensaver', function () {
        return function (scope, element) {
            new screensaver(element).run();
        }
    })

}).call(this);