(function () {
    "use strict";

    var screensaver = function () {

        this.restrict = 'EA';
        this.replace = true;
        this.templateUrl = 'Views/Templates/screensaver.html';
        this.scope = true;
        this.link = function (sc, el) {
            scope = sc;
            element = el;
            run();
        }

        var timeout,
            activeImage,
            inactiveImage,
            element,
            scope,
            self = this;

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
            animatePhotos();
            showScreensaver();
            resetTimeout(5000);
            fetchRandomImage();
        }

        function animatePhotos() {
            var visibleImg = element.find('.current-image'),
                hiddenImg = element.find(':not(.current-image)');

            hiddenImg.attr('src', inactiveImage.src);
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
                inactiveImage = new Image();
                inactiveImage.src = images[random].url;
            });
        }
    }

    var app = angular.module('Croom');
    app.directive('screensaver', function () {
        return new screensaver();
    })

}).call(this);