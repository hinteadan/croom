(function () {
    "use strict";

    var app = angular.module('Croom', ['ngResource','ngRoute'])
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

}).call(this);