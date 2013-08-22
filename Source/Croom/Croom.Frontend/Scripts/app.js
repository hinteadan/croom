(function () {
    "use strict";

    var app = angular.module('Croom', ['ngResource'])
        .config(function ($routeProvider) {
          $routeProvider
            .when('/', {
                templateUrl: 'views/home.html',
                controller: 'home'
            })
            .otherwise({
                redirectTo: '/'
            });
        });

    app.run(function(){
        var screensaver = new window.screensaver();
        screensaver.run();
    });

}).call(this);