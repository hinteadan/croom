(function () {
    "use strict";

    var screensaver = function (element) {

        var timeout;

        function run() {
            $(document).on("mousemove keydown click", function () {
                hideScreensaver();
                clearTimeout(this.timeout);
                this.timeout = setTimeout(function () {
                    displayScreensaver();
                }, 5 * 1000);
            }).click();
        }

        function displayScreensaver() {
            element.addClass('screensaver-active');
        }
        function hideScreensaver() {
            element.removeClass('screensaver-active');
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