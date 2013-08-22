(function () {
    "use strict";

    window.screensaver = function() {

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
            $("#screensaver").addClass('screensaver-active');
        }
        function hideScreensaver() {
            $("#screensaver").removeClass('screensaver-active');
        }

        return {
            run: run
        };
    }
}).call(this);